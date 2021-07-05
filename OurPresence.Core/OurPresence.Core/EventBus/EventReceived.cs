// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MediatR;

namespace OurPresence.Core.EventBus
{
    public class EventReceived<TE> : INotification
    {
        public EventReceived(TE @event)
        {
            Event = @event;
        }

        public TE Event { get; }
    }
}
