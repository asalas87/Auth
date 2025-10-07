using ErrorOr;
using MediatR;

namespace Application.Notifications.Querys;

public record IsAlreadySentQuery(Guid DocumentId) : IRequest<ErrorOr<bool>>;
