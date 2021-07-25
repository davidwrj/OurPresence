using System;
using System.ComponentModel;

namespace OurPresence.Modeller.Fluent
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BehaviourResponseBuilder<TBuilder>
    {
        public BehaviourResponseBuilder(TBuilder builder, Domain.BehaviourResponse response)
        {
            Build = builder;
            Instance = response ?? throw new ArgumentNullException(nameof(response));
        }

        public TBuilder Build { get; }

        public Domain.BehaviourResponse Instance { get; }

        public FieldBuilder<BehaviourResponseBuilder<TBuilder>> AddField(string name)
        {
            var field = Field<BehaviourResponseBuilder<TBuilder>>.Create(this, name);
            Instance.Fields.Add(field.Instance);
            return field;
        }

        public BehaviourResponseBuilder<TBuilder> IsFileStream()
        {
            Instance.IsFileStream = true;
            return this;
        }

        public BehaviourResponseBuilder<TBuilder> IsCollection()
        {
            Instance.IsCollection = true;
            return this;
        }
    }
}
