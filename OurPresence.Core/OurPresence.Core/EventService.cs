// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Core.EventBus;
using OurPresence.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OurPresence.Core
{
    public class EventsService<TA, TKey> : IEventsService<TA, TKey> where TA : class, IAggregateRoot<TKey>
    {
        private readonly IEventsRepository<TA, TKey> _eventsRepository;
        private readonly IEventProducer<TA, TKey> _eventProducer;

        public EventsService(IEventsRepository<TA, TKey> eventsRepository, IEventProducer<TA, TKey> eventProducer)
        {
            _eventsRepository = eventsRepository ?? throw new ArgumentNullException(nameof(eventsRepository));
            _eventProducer = eventProducer ?? throw new ArgumentNullException(nameof(eventProducer));
        }

        public async Task PersistAsync(TA aggregateRoot)
        {
            if (null == aggregateRoot)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            await _eventsRepository.AppendAsync(aggregateRoot);
            await _eventProducer.DispatchAsync(aggregateRoot);

            aggregateRoot.ClearEvents();
        }

        public Task<TA> RehydrateAsync(TKey key)
        {
            return _eventsRepository.RehydrateAsync(key);
        }
    }
}
