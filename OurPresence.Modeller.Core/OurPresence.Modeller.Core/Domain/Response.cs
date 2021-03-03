using Newtonsoft.Json;
using System.Collections.Generic;

namespace OurPresence.Modeller.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Response
    {
        [JsonProperty]
        public List<Field> Fields { get; } = new List<Field>();
    }
}