using ErrorOr;
using MediatR;

namespace Application.Documents.Renovation.Update;
public record UpdateRenovationCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public Guid AssignedToId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string EmployerName { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
