// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Fluent
{
    public static class Behaviour<TBuilder>
    {
        public static BehaviourBuilder<TBuilder> Create(TBuilder model, string name)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(name);

            var behaviour = new Domain.Behaviour(name);
            return new BehaviourBuilder<TBuilder>(model, behaviour);
        }
    }
}
