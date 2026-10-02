using Application.Interfaces;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Enums;
using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecification.Update;

public sealed class UpdateProcedureSpecificationCommandHandler(
    IProcedureSpecificationRepository procedureSpecificationRepository,
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService) : IRequestHandler<UpdateProcedureSpecificationCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(UpdateProcedureSpecificationCommand request, CancellationToken cancellationToken)
    {
        if (await procedureSpecificationRepository.GetByIdAsync(new DocumentFileId(request.Id)) is not Domain.Documents.Entities.ProcedureSpecification document ||
            document.DocumentType != DocumentType.WeldingProcedure)
        {
            return Error.NotFound("ProcedureSpecification.NotFound", "The procedure specification was not found.");
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
            newRelativePath = await fileStorageService.SaveAsync(request.File, DocumentType.WeldingProcedure, fileName, cancellationToken);
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

        procedureSpecificationRepository.Update(document);

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