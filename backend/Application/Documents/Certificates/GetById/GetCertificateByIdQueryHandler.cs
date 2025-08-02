using Application.Documents.Certificate.DTOs;
using Application.Documents.Certificate.GetById;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificates.GetById;
public class GetCertificateByIdQueryHandler : IRequestHandler<GetCertificateByIdQuery, ErrorOr<CertificateResponseDTO>>
{
    private readonly ICertificateRepository _certificateRepository;
    public GetCertificateByIdQueryHandler(ICertificateRepository certificateRepository)
    {
        _certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }
    public async Task<ErrorOr<CertificateResponseDTO>> Handle(GetCertificateByIdQuery request, CancellationToken cancellationToken)
    {
        var certificate = await _certificateRepository.GetByIdAsync(request.Id);
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
            Path = certificate.Path,
            UploadDate = certificate.UploadDate,
            UploadedBy = certificate.UploadedBy.Name,
            AssignedTo = certificate.AssignedTo?.Name ?? string.Empty,
            ValidFrom = certificate.ValidFrom,
            ValidUntil = certificate.ValidUntil
        };
    }
}
