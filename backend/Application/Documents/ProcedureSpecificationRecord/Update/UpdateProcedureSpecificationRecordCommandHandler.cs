using Application.Interfaces;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Enums;
using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecificationRecord.Update;

public sealed class UpdateProcedureSpecificationRecordCommandHandler(
    IProcedureSpecificationRecordRepository repository,
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService) : IRequestHandler<UpdateProcedureSpecificationRecordCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(UpdateProcedureSpecificationRecordCommand request, CancellationToken cancellationToken)
    {
        if (await repository.GetByIdAsync(new DocumentFileId(request.Id)) is not Domain.Documents.Entities.ProcedureSpecificationRecord document ||
            document.DocumentType != DocumentType.ProcedureSpecificationRecord)
        {
            return Error.NotFound("ProcedureSpecificationRecord.NotFound", "The procedure specification record was not found.");
        }

        Company? assignedCompany = null;
        if (request.AssignedToId is Guid assignedToId)
        {
            assignedCompany = await companyRepository.GetByIdAsync(new CompanyId(assignedToId), cancellationToken);
            if (assignedCompany is null)
            {
                return Error.NotFound("Company.NotFound", "The company was not found.");
            }
        }

        var previousRelativePath = document.RelativePath;
        string? newRelativePath = null;
        if (request.File is not null)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var fileName = $"{timestamp}_{request.ProcedureNumber}.pdf";
            newRelativePath = await fileStorageService.SaveAsync(request.File, DocumentType.ProcedureSpecificationRecord, fileName, cancellationToken);
        }

        document.Update(
            request.Name,
            request.Description ?? string.Empty,
            request.ExpirationDate,
            assignedCompany,
            request.ProcedureNumber,
            request.StandardCode);

        if (newRelativePath is not null)
        {
            document.ReplaceFile(newRelativePath);
        }

        repository.Update(document);

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            if (newRelativePath is not null)
            {
                await fileStorageService.DeleteAsync(newRelativePath);
            }

            throw;
        }

        if (newRelativePath is not null)
        {
            await fileStorageService.DeleteAsync(previousRelativePath);
        }

        return document.Id.Value;
    }
}