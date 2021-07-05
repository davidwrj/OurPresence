// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OurPresence.Modeller.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Request : NamedElementBase
    {
        private Response? _response;

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}";

        void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name must be passed", nameof(name));
            }
            OverrideName(name);
        }

        public Request(string name, Response? response = null)
            : base(name)
        {
            Response = response;
            SetName(name);
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Response? Response
        {
            get => _response;
            set
            {
                _response = value;
                SetName(Name.Value);
            }
        }

        [JsonProperty]
        public List<Field> Fields { get; } = new List<Field>();
    }
}