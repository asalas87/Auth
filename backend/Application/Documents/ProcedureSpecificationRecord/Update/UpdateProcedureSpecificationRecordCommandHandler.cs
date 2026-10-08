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
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProcedureSpecificationRecordCommand, ErrorOr<Guid>>
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

        document.Update(
            assignedCompany,
            request.ProcedureNumber,
            request.StandardCode);

        repository.Update(document);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return document.Id.Value;
    }
}
