using System;
using System.ComponentModel;

namespace OurPresence.Modeller.Fluent
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BehaviourRequestBuilder<TBuilder>
    {
        public BehaviourRequestBuilder(TBuilder builder, Domain.BehaviourRequest request)
        {
            Build = builder;
            Instance = request ?? throw new ArgumentNullException(nameof(request));
        }

        public TBuilder Build { get; }

        public Domain.BehaviourRequest Instance { get; }

        public FieldBuilder<BehaviourRequestBuilder<TBuilder>> AddField(string name)
        {
            var field = Field<BehaviourRequestBuilder<TBuilder>>.Create(this, name);
            Instance.Fields.Add(field.Instance);
            return field;
        }

        public BehaviourRequestBuilder<TBuilder> Route(string v)
        {
            Instance.Route = v;
            return this;
        }
    }
}
