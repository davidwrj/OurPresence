// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Fluent
{
    public static class Field<TBuilder>
    {
        public static FieldBuilder<TBuilder> Create(TBuilder model, string name)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(name);

            var field = new Domain.Field(name);
            return new FieldBuilder<TBuilder>(model, field);
        }
    }
}
