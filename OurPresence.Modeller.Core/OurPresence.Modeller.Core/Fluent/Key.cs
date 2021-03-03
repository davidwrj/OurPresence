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
