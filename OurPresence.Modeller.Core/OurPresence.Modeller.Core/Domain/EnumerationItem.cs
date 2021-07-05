// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Humanizer;
using Newtonsoft.Json;
using System.Diagnostics;

namespace OurPresence.Modeller.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EnumerationItem
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name} [{Value}]";

        public EnumerationItem(int value, string name)
            : this(value, name, name.Humanize().Transform(To.TitleCase))
        {
        }

        [JsonConstructor]
        public EnumerationItem(int value, string name, string display)
        {
            Value = value;
            Name = name;
            Display = display;
        }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Display { get; set; }

        [JsonProperty]
        public int Value { get; set; }
    }

}