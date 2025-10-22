namespace Domain.Primitives
{
    public abstract class AggergateRoot<TId> : Entity<TId>, IAggregateRoot
    {
        private readonly List<DomainEvent> _domainEvents = [];

        public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents;
        public void Raise(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
