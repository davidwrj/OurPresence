// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Fluent
{
    public static class IndexField
    {
        public static IndexFieldBuilder Create(IndexBuilder builder, string name)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            var field = new Domain.IndexField(name);
            return new IndexFieldBuilder(builder, field);
        }
    }
}
