// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Fluent
{
    public static class Model
    {
        public static ModelBuilder Create(ModuleBuilder module, string name)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(name);
            }

            var model = new Domain.Model(name);
            return new ModelBuilder(module, model);
        }
    }
}
