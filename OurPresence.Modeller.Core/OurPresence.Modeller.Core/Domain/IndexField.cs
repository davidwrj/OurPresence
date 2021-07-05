// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;

namespace OurPresence.Modeller.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class IndexField : NamedElementBase
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}";

        public IndexField(string name)
            : base(name)
        { }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(ListSortDirection.Ascending)]
        public ListSortDirection Sort { get; set; } = ListSortDirection.Ascending;
    }

}