using System;
using System.ComponentModel;
using OurPresence.Modeller.Domain;

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

        public BehaviourBuilder<T> AltersDomain(BehaviourVerb value)
        {
            Instance.Verb = value;
            return this;
        }

        public BehaviourRequestBuilder<BehaviourBuilder<T>> AddRequest(string name)
        {
            var request = BehaviourRequest<BehaviourBuilder<T>>.Create(this, name);
            Instance.Request = request.Instance;
            return request;
        }
   
        public BehaviourResponseBuilder<BehaviourBuilder<T>> AddResponse(string name)
        {
            var response = BehaviourResponse<BehaviourBuilder<T>>.Create(this, name);
            Instance.Response = response.Instance;
            return response;
        }
    }
}
