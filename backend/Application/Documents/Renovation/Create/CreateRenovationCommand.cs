using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Renovation.Create;
public record CreateRenovationCommand : IRequest<ErrorOr<Guid>>
{
    public DateTime Validity { get; set; }
    public DateTime ExpirationDate { get; set; }
    public Guid UploadedById { get; set; }
    public Guid AssignedToId { get; set; }
    public IFormFile File { get; set; } = default!;
    public string CertificateNumber { get; set; } = string.Empty;
    public string EmployerFullName { get; set; } = string.Empty;
    public string StandardCode { get; set; } = string.Empty;
    public int RenovationNumber { get; set; }
}
