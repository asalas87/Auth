using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.Delete;

public record DeleteCertificateCommand(Guid Id) : IRequest<ErrorOr<Guid>>;
