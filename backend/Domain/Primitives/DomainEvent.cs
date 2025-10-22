using MediatR;

namespace Domain.Primitives;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}
