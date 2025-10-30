using Domain.Primitives;

namespace Domain.Security.Events;

public record UserCreatedEvent(Guid UserId, string Email) : DomainEvent;
