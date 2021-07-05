// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Fluent
{
    public static class Key
    {
        public static KeyBuilder Create(ModelBuilder modelBuilder, Domain.Model model)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return new KeyBuilder(modelBuilder, model);
        }
    }
}
