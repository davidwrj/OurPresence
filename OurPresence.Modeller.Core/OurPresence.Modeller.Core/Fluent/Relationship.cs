// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Fluent
{
    public static class Relationship
    {
        public static RelationshipBuilder Create(ModuleBuilder module, ModelBuilder model)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var relation = new Domain.Relationship();
            return new RelationshipBuilder(module, model, relation);
        }
    }
}
