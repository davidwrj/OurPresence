// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OurPresence.Modeller.Domain
{
    public class Key
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Field> Fields { get; } = new List<Field>();
    }
}