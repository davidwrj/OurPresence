// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Core.EventBus
{
    public static class EventReceivedFactory
    {
        public static EventReceived<TE> Create<TE>(TE @event)
        {
            return new EventReceived<TE>(@event);
        }
    }
}
