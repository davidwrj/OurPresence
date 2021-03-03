using System;
using System.ComponentModel;

namespace OurPresence.Modeller.Fluent
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class KeyBuilder : FluentBase
    {
        public KeyBuilder(ModelBuilder modelBuilder, Domain.Model model)
        {
            Build = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));
            Instance = model ?? throw new ArgumentNullException(nameof(model));
        }

        public ModelBuilder Build { get; }

        public Domain.Model Instance { get; }

        public FieldBuilder<KeyBuilder> AddField(string name)
        {
            var field = Fluent.Field<KeyBuilder>.Create(this, name);
            Instance.Key.Fields.Add(field.Instance);
            return field;
        }
    }
}
