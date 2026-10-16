using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.ProcedureSpecificationRecord.Update;

public record UpdateProcedureSpecificationRecordCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; set; }
    public Guid? AssignedToId { get; set; }
    public string ProcedureNumber { get; set; } = string.Empty;
    public string StandardCode { get; set; } = string.Empty;
}
