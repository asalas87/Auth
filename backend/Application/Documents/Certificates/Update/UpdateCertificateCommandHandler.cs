using Domain.Documents.Interfaces;
using Domain.Partners.Entities;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.Update;

public sealed class UpdateCertificateCommandHandler : IRequestHandler<UpdateCertificateCommand, ErrorOr<Guid>>
{
    private readonly ICertificateRepository _certificateRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCertificateCommandHandler(
        ICertificateRepository documentRepository,
        ICompanyRepository ICompanyRepository,
        IUnitOfWork unitOfWork)
    {
        _certificateRepository = documentRepository;
        _companyRepository = ICompanyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Guid>> Handle(UpdateCertificateCommand request, CancellationToken cancellationToken)
    {

        if (await _certificateRepository.GetByIdAsync(request.Id) is not Domain.Documents.Entities.Certificate certificate)
        {
            return Error.NotFound("Certificate.NotFound", "The certificate with the provide Id was not found.");
        }

        if (await _companyRepository.GetByIdAsync(new CompanyId(request.AssignedTo)) is not Company assignedComapny)
        {
            return Error.NotFound("Company.NotFound", "The user with the provide Id was not found.");
        }

        certificate.Update(
            request.Name,
            request.ValidFrom,
            request.ValidUntil,
            assignedComapny);

        _certificateRepository.Update(certificate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return certificate.Id.Value;
    }
}
