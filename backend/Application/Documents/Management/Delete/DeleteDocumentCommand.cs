using ErrorOr;
using MediatR;

namespace Application.Documents.Management.Delete;

public record DeleteDocumentCommand(Guid Id) : IRequest<ErrorOr<Guid>>;
