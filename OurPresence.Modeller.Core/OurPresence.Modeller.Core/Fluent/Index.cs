// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Fluent
{
    public static class Index
    {
        public static IndexBuilder Create(ModelBuilder model, string name)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("message", nameof(name));
            var index = new Domain.Index(name);
            return new IndexBuilder(model, index);
        }
    }
}
