using ErrorOr;
using MediatR;

namespace Application.Documents.Renovation.Update;
public record UpdateRenovationCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; set; }
    public DateTime Validity { get; set; }
    public Guid AssignedToId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string EmployerFullName { get; set; } = string.Empty;
    public string StandardCode { get; set; } = string.Empty;
}
