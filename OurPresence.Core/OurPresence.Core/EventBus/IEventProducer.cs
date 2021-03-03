using OurPresence.Core.Models;
using System.Threading.Tasks;

namespace OurPresence.Core.EventBus
{
    public interface IEventProducer<in TA, in TKey>
    where TA : IAggregateRoot<TKey>
    {
        Task DispatchAsync(TA aggregateRoot);
    }
}
