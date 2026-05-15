using Domain.Primitives;

namespace Domain.Security.Events;

public record UserCreatedEvent(Guid UserId, string Email,  Guid CompanyId) : DomainEvent;
