using OurPresence.Core.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace OurPresence.Core.EventBus
{
    public class EventConsumerFactory : IEventConsumerFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventConsumerFactory(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }

        public IEventConsumer Build<TA, TKey>() where TA : IAggregateRoot<TKey>
        {
            using var scope = _scopeFactory.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<IEventConsumer<TA, TKey>>();

            async Task OnEventReceived(object s, IDomainEvent<TKey> e)
            {
                var @event = EventReceivedFactory.Create((dynamic)e);

                using var innerScope = _scopeFactory.CreateScope();
                var mediator = innerScope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(@event, CancellationToken.None);
            }
            consumer.EventReceived += OnEventReceived;

            return consumer;
        }
    }
}
