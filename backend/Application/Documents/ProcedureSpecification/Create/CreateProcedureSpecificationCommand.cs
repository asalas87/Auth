using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.ProcedureSpecification.Create;

public record CreateProcedureSpecificationCommand : IRequest<ErrorOr<Guid>>
{
    public Guid UploadedById { get; set; }
    public Guid? AssignedToId { get; set; }
    public IFormFile File { get; set; } = null!;
    public string ProcedureNumber { get; set; } = string.Empty;
    public string StandardCode { get; set; } = string.Empty;
}
