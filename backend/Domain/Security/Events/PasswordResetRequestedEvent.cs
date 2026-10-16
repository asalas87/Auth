using Domain.Primitives;

namespace Domain.Security.Events;

public record PasswordResetRequestedEvent(Guid UserId, string Name, string Email, Guid CompanyId) : DomainEvent;
