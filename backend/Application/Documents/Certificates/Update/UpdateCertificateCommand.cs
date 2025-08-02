using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.Update;
public record UpdateCertificateCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public Guid AssignedTo { get; set; }
}
