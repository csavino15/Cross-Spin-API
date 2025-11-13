using Domain.Abstractions;

namespace Application.Abstractions.Messaging;
public interface IMessageService
{
    void CreateMessage(IDomainEvent domainEvent);
    void CreateMessages(IEnumerable<IDomainEvent> domainEvents);
}
