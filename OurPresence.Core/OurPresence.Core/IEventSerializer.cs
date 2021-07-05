// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Core.Models;

namespace OurPresence.Core
{
    public interface IEventSerializer
    {
        IDomainEvent<TKey> Deserialize<TKey>(string type, byte[] data);
        IDomainEvent<TKey> Deserialize<TKey>(string type, string data);
        byte[] Serialize<TKey>(IDomainEvent<TKey> @event);
    }
}
