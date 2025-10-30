using MediatR;

namespace Domain.Primitives
{
    public interface IDomainEvent : INotification
    {
        record DomainEvent(Guid Id);
    }
}
