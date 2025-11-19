using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Documents.Renovation.Update;
public sealed class UpdateRenovationCommandHandler(
    IRenovationRepository renovationRepository,
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateRenovationCommand, ErrorOr<Guid>>
{
    private readonly IRenovationRepository _renovationRepository = renovationRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Guid>> Handle(UpdateRenovationCommand request, CancellationToken cancellationToken)
    {
        if (await _renovationRepository.GetByIdAsync(new DocumentFileId(request.Id)) is not Domain.Documents.Entities.Renovation renovation)
        {
            return Error.NotFound("Renovation.NotFound", "The renovation with the provided Id was not found.");
        }

        if (await _companyRepository.GetByIdReadOnlyAsync(new CompanyId(request.AssignedToId), cancellationToken) is not Company assignedCompany)
        {
            return Error.NotFound("Company.NotFound", "The company with the provided Id was not found.");
        }

        renovation.Update(
            request.CertificateNumber,
            request.EmployerName,
            request.Code,
            request.ValidFrom,
            request.ValidUntil,
            assignedCompany);

        _renovationRepository.Update(renovation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return renovation.Id.Value;
    }
}
