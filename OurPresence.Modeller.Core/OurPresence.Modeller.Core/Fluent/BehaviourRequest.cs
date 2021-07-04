// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Fluent
{
    public static class BehaviourRequest<TBuilder>
    {
        public static BehaviourRequestBuilder<TBuilder> Create(TBuilder model, string name)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model));

            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(name);

            var behaviour = new Domain.BehaviourRequest(name);
            return new BehaviourRequestBuilder<TBuilder>(model, behaviour);
        }
    }
}
