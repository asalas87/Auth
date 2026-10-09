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
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProcedureSpecificationCommand, ErrorOr<Guid>>
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

        document.Update(
            assignedCompany,
            request.ProcedureNumber,
            request.StandardCode);

        procedureSpecificationRepository.Update(document);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return document.Id.Value;
    }
}
