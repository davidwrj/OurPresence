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
