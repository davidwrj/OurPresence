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
