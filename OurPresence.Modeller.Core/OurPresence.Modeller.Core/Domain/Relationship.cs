using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OurPresence.Modeller.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Relationship
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string DebuggerDisplay => ToString();

        public void SetRelationship(Name principalModel, IEnumerable<Name> principalFields, Name dependantModel, IEnumerable<Name> dependantFields, RelationshipTypes principalType = RelationshipTypes.One, RelationshipTypes dependantType = RelationshipTypes.Many, string? linkTable = null)
        {
            if (principalFields is null)
                throw new ArgumentNullException(nameof(principalFields));
            if (dependantFields is null)
                throw new ArgumentNullException(nameof(dependantFields));
            PrincipalModel = principalModel ?? throw new ArgumentNullException(nameof(principalModel));
            PrincipalFields = new List<Name>(principalFields);
            DependantModel = dependantModel ?? throw new ArgumentNullException(nameof(dependantModel));
            DependantFields = new List<Name>(dependantFields);
            PrincipalType = principalType;
            DependantType = dependantType;

            LinkTable = PrincipalType == RelationshipTypes.Many && DependantType == RelationshipTypes.Many && string.IsNullOrWhiteSpace(linkTable)
                ? principalModel.Value + dependantModel.Value
                : linkTable;
        }

        [JsonProperty]
        public Name PrincipalModel { get; set; } = new Name("");

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Name> PrincipalFields { get; set; } = new List<Name>();

        [JsonProperty]
        public RelationshipTypes PrincipalType { get; set; }

        [JsonProperty]
        public Name DependantModel { get; set; } = new Name("");

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Name> DependantFields { get; set; } = new List<Name>();

        [JsonProperty]
        public RelationshipTypes DependantType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? LinkTable { get; set; }

        public override string ToString()
        {
            return $"[{PrincipalModel}].[{string.Join(",", PrincipalFields)}] ({PrincipalType}) -> [{DependantModel}].[{string.Join(",", DependantFields)}] ({DependantType})";
        }
    }
}