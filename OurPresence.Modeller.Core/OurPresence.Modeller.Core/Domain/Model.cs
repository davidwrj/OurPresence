using OurPresence.Modeller.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;

namespace OurPresence.Modeller.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Model : NamedElementBase, INamedElement
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}";

        internal void FinaliseNames()
        { }

        public Model(string name) 
            : base(name)
        { }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Schema { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Key Key { get; set; } = new Key();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(true)]
        public bool HasAudit { get; set; } = true;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Field> Fields { get; } = new List<Field>();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Index> Indexes { get; } = new List<Index>();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Relationship> Relationships { get; } = new List<Relationship>();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Behaviour> Behaviours { get; } = new List<Behaviour>();

        [JsonIgnore]
        public IEnumerable<Field> AllFields => Key.Fields.Concat(Fields);

        string INamedElement.Name => Name.Value;

        internal Field GetField(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Field name must be supplied.", nameof(name));
            return AllFields.FirstOrDefault(f => f.Name.Value == name) ?? throw new ArgumentNullException(nameof(name), $"Field {name} not found.");
        }
    }
}