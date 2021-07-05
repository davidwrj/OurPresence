// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Core.Models;

namespace OurPresence.Core.EventBus
{
    public interface IEventConsumerFactory
    {
        IEventConsumer Build<TA, TKey>() where TA : IAggregateRoot<TKey>;
    }
}
