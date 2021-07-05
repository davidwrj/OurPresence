// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
