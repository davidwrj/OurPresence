using System;
using System.ComponentModel;

namespace OurPresence.Modeller.Fluent
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BehaviourBuilder<T>
    {
        public BehaviourBuilder(T builder, Domain.Behaviour field)
        {
            Build = builder;
            Instance = field ?? throw new ArgumentNullException(nameof(field));
        }

        public T Build { get; }

        public Domain.Behaviour Instance { get; }

        public BehaviourBuilder<T> Raising(string @event)
        {
            Instance.Event = @event;
            return this;
        }

        public BehaviourBuilder<T> AltersDomain(bool value = true)
        {
            Instance.AltersDomain = value;
            return this;
        }

        public FieldBuilder<BehaviourBuilder<T>> AddField(string name)
        {
            var field = Field<BehaviourBuilder<T>>.Create(this, name);
            Instance.Fields.Add(field.Instance);
            return field;
        }
    }
}
