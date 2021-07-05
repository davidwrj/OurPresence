// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Core.Models
{
    public interface IDomainEvent<out TKey>
    {
        long AggregateVersion { get; }
        TKey AggregateId { get; }
        DateTime Timestamp { get; }
    }
}
