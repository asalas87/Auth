using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Certificate.Create;
public record CreateCertificateCommand : IRequest<ErrorOr<Guid>>
{
    public string Name { get; set; } = default!;
    public DateTime ValidFrom { get; set; }
    public DateTime ExpirationDate { get; set; }
    public Guid UploadedById { get; set; }
    public Guid AssignedToId { get; set; }
    public IFormFile File { get; set; } = default!;
    public string CertificateNumber { get; set; } = string.Empty;
    public string EmployerName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
