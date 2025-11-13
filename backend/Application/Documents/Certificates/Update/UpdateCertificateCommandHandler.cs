using Application.Controls.Interfaces;
using Application.Documents.Certificate.Update;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Partners.Entities;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificates.Update;

public sealed class UpdateCertificateCommandHandler(
    ICertificateRepository documentRepository,
    ICompanyRepository ICompanyRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCertificateCommand, ErrorOr<Guid>>
{
    private readonly ICertificateRepository _certificateRepository = documentRepository;
    private readonly ICompanyRepository _companyRepository = ICompanyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Guid>> Handle(UpdateCertificateCommand request, CancellationToken cancellationToken)
    {

        if (await _certificateRepository.GetByIdAsync(new DocumentFileId(request.Id)) is not Domain.Documents.Entities.Certificate certificate)
        {
            return Error.NotFound("Certificate.NotFound", "The certificate with the provide Id was not found.");
        }

        if (await _companyRepository.GetByIdReadOnlyAsync(new CompanyId(request.AssignedToId), cancellationToken) is not Company assignedComapny)
        {
            return Error.NotFound("Company.NotFound", "The user with the provide Id was not found.");
        }

        certificate.Update(
            request.CertificateNumber,
            request.EmployerName,          
            request.Code,
            request.ValidFrom,
            request.ValidUntil,
            assignedComapny);

        _certificateRepository.Update(certificate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return certificate.Id.Value;
    }
}
