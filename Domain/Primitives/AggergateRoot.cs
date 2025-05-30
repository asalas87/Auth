namespace Domain.Primitives
{
    public abstract class AggergateRoot<TId> : Entity<TId>, IAggregateRoot
    {
        private readonly List<DomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents;
        protected void Raise(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
