using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecification.Update;

public record UpdateProcedureSpecificationCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; set; }
    public Guid? AssignedToId { get; set; }
    public string ProcedureNumber { get; set; } = string.Empty;
    public string StandardCode { get; set; } = string.Empty;
}
