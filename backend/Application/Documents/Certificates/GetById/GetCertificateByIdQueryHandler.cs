using Application.Documents.Certificate.Dtos;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.GetById;
public class GetCertificateByIdQueryHandler : IRequestHandler<GetCertificateByIdQuery, ErrorOr<CertificateResponseDTO>>
{
    private readonly ICertificateRepository _certificateRepository;
    public GetCertificateByIdQueryHandler(ICertificateRepository certificateRepository)
    {
        _certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }
    public async Task<ErrorOr<CertificateResponseDTO>> Handle(GetCertificateByIdQuery request, CancellationToken cancellationToken)
    {
        var certificate = await _certificateRepository.GetByIdAsync(new DocumentFileId(request.Id));
        if (certificate is null)
        {
            return Error.NotFound("Certificate not found.");
        }
        return new CertificateResponseDTO
        {
            Id = certificate.Id.Value,
            Name = certificate.Name,
            Description = certificate.Description,
            ExpirationDate = certificate.ExpirationDate,
            RelativePath = certificate.RelativePath,
            UploadDate = certificate.UploadDate,
            UploadedBy = certificate.UploadedBy.Name,
            AssignedToId = certificate.AssignedTo?.Id.Value ?? Guid.Empty,
            ValidFrom = certificate.ValidFrom,
            CertificateNumber = certificate.CertificateNumber,
            EmployerName = certificate.EmployerName,
            Code = certificate.Code
        };
    }
}
