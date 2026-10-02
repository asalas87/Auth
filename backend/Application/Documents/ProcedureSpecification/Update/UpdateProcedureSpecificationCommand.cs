using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.ProcedureSpecification.Update;

public record UpdateProcedureSpecificationCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; set; }
    public Guid? AssignedToId { get; set; }
    public IFormFile? File { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string ProcedureNumber { get; set; } = string.Empty;
    public string StandardCode { get; set; } = string.Empty;
}