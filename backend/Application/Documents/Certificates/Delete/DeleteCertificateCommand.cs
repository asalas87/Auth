using ErrorOr;
using MediatR;

namespace Application.Documents.Certificates.Delete;

public record DeleteCertificateCommand(Guid Id) : IRequest<ErrorOr<Guid>>;
