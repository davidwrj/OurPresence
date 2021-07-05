// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Fluent
{
    public static class Enumeration
    {
        public static EnumerationBuilder Create(ModuleBuilder module, string name)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(name);

            var enumeration = new Domain.Enumeration(name);
            return new EnumerationBuilder(module, enumeration);
        }
    }
}
