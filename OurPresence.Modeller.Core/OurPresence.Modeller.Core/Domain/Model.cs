// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace OurPresence.Modeller.Domain
{
    [Flags]
    public enum CRUDSupport
    {
        None = 0,
        Create = 1,
        Read = 2,
        Update = 4,
        Delete = 8,

        All = 15
    }

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
        [DefaultValue(false)]
        public bool HasAudit { get; set; } = false;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool IsRoot { get; set; } = false;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(CRUDSupport.All)]
        public CRUDSupport SupportCrud { get; set; } = CRUDSupport.None;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Field> Fields { get; } = new List<Field>();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Index> Indexes { get; } = new List<Index>();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Relationship> Relationships { get; } = new List<Relationship>();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Behaviour> Behaviours { get; } = new List<Behaviour>();

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
