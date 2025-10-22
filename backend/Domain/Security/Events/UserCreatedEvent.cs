using Domain.Primitives;

namespace Domain.Security.Events;

public record UserCreatedEvent(Guid Id, string Email) : DomainEvent;
