namespace Domain.Primitives;
public interface IAggregateRoot
{
    IReadOnlyCollection<DomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
