using ErrorOr;
using MediatR;

namespace Application.Notifications.Querys;

public record IsAlreadySentForCompanyQuery(Guid companyId) : IRequest<ErrorOr<bool>>;
