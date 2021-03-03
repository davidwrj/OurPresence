using OurPresence.Core.Models;

namespace OurPresence.Core.EventBus
{
    public interface IEventConsumerFactory
    {
        IEventConsumer Build<TA, TKey>() where TA : IAggregateRoot<TKey>;
    }
}
