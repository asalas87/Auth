using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Certificate.Create;
public record CreateCertificateCommand : IRequest<ErrorOr<Guid>>
{
    public DateTime Validity { get; set; }
    public Guid UploadedById { get; set; }
    public Guid AssignedToId { get; set; }
    public IFormFile File { get; set; } = default!;
    public string CertificateNumber { get; set; } = string.Empty;
    public string EmployerFullName { get; set; } = string.Empty;
    public string StandardCode { get; set; } = string.Empty;
}
