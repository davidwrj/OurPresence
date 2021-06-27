using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;

namespace OurPresence.Modeller.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Field : NamedElementBase
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}";

        public Field(string name) : this(name, DataTypes.String, false)
        { }

        [JsonConstructor]
        public Field(string name, DataTypes dataType, bool nullable)
            : base(name)
        {
            DataType = dataType;
            Nullable = nullable;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Default { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxLength { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MinLength { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Precision { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Scale { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(DataTypes.String)]
        public DataTypes DataType { get; set; } = DataTypes.String;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? DataTypeTypeName { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool Nullable { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(false)]
        public bool BusinessKey { get; set; }
    }
}
