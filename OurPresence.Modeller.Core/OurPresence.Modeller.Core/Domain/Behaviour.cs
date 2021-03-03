using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace OurPresence.Modeller.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Behaviour : NamedElementBase
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}";

        [JsonConstructor]
        public Behaviour(string name)
            : base(name)
        { }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Field> Fields { get; } = new List<Field>();

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Event { get; set; }
    }
}