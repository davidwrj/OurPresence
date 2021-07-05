// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;

namespace OurPresence.Modeller.Domain
{
    public abstract class NamedElementBase
    {
        protected NamedElementBase(string name)
        {
            Name = new Name(name);
        }

        protected void OverrideName(string name)
        {
            Name.SetName(name);
        }

        [JsonProperty]
        public Name Name { get; protected set; }
    }
}