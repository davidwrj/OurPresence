// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using System.ComponentModel;
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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BehaviourRequest? Request { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BehaviourResponse? Response { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Event { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(BehaviourVerb.Get)]
        public BehaviourVerb Verb { get; set; } 
    }
}
