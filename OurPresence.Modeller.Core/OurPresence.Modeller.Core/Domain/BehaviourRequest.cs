using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace OurPresence.Modeller.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class BehaviourRequest : NamedElementBase
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}";

        [JsonConstructor]
        public BehaviourRequest(string name)
            : base(name)
        {
            Route = name.ToLower();
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Field> Fields { get; } = new List<Field>();

        [JsonProperty]
        public string Route { get; set; }
    }
}
