using Domain.Primitives;

namespace Domain.Security.Events;

public record UserCreatedEvent(Guid UserId, string Name, string Email,  Guid CompanyId) : DomainEvent;
