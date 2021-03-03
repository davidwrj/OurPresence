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
            if (name != null && name.Trim().Length > 0)
            {
                if (name.EndsWith("Command", StringComparison.InvariantCultureIgnoreCase))
                    name = name.Substring(0, name.Length - 7);
                if (name.EndsWith("Query", StringComparison.InvariantCultureIgnoreCase))
                    name = name.Substring(0, name.Length - 5);
                if (name.Trim().Length > 0)
                    if (Response == null)
                        name += "Command";
                    else
                        name += "Query";
            }
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be passed", nameof(name));

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