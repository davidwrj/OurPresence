// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Core.Models;
using System.Threading.Tasks;

namespace OurPresence.Core
{
    public interface IEventsService<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        Task PersistAsync(TA aggregateRoot);
        Task<TA> RehydrateAsync(TKey key);
    }
}
