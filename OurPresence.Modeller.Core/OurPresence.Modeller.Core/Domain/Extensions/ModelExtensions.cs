using System.Linq;

namespace OurPresence.Modeller.Domain.Extensions
{
    public static class ModelExtensions
    {
        public static bool HasActive(this Model model) => model.Fields.FirstOrDefault(f => f.Name.Singular.Value == "IsActive") != null;

        public static Field? HasBusinessKey(this Model model) => model.Fields.FirstOrDefault(f => f.BusinessKey);

        public static bool IsEntity(this Model model) => model?.Key?.Fields != null && model.Key.Fields.Count == 1 && model.Key.Fields[0].Name.Singular.Value == "Id" && model.Key.Fields[0].DataType == DataTypes.UniqueIdentifier;

        public static Relationship? GetForeignKey(this Model principal, Model dependant)
        {
            if (principal == null || dependant == null) return null;

            var fields = principal.Key.Fields.Select(f => dependant.Name.Value + f.Name.Value);
            return principal.Relationships.FirstOrDefault(r => r.PrincipalModel == principal.Name && r.DependantModel == dependant.Name && r.DependantFields.Select(f => f.Value).SequenceEqual(fields));
        }

        public static void MakeEntity(this Model model)
        {
            if (model.Key.Fields.Any())
                model.Key.Fields.RemoveAll(f => f.Name.ToString() != "Id");

            if (model.Key.Fields.Any())
            {
                var field = model.Key.Fields.First();
                field.DataType = DataTypes.UniqueIdentifier;
                field.Nullable = false;
            }
            else
                AddDefaultKey(model);
        }

        public static Model AddRelation(this Model model, string[] principalFields, string dependant, string[] dependantFields)
        {
            return AddRelation(model, principalFields, dependant, dependantFields, RelationshipTypes.One, RelationshipTypes.Many, null);
        }

        public static Model AddRelation(this Model model, string[] principalFields, string dependant, string[] dependantFields, RelationshipTypes principalType, RelationshipTypes dependantType, string? linkTable)
        {
            model.Relationships.Add(new Relationship()
            {
                PrincipalFields = principalFields.Select(f => new Name(f)).ToList(),
                PrincipalModel = model.Name,
                PrincipalType = principalType,
                DependantFields = dependantFields.Select(f => new Name(f)).ToList(),
                DependantModel = new Name(dependant),
                DependantType = dependantType,
                LinkTable = linkTable
            });
            return model;
        }

        public static Model AddDefaultKey(this Model model)
        {
            model.Key.Fields.Add(new Field("Id") { DataType = DataTypes.UniqueIdentifier, Nullable = false });
            return model;
        }
        public static Model AddIndex(this Model model, string name, bool isUnique = true, bool isClusted = false, params string[] fieldNames)
        {
            var idx = new Domain.Index(name) { IsUnique = isUnique, IsClustered = isClusted };
            foreach (var fieldName in fieldNames)
                idx.Fields.Add(new IndexField(fieldName));
            model.Indexes.Add(idx);
            return model;
        }


        public static Model AddFieldString(this Model model, string name, int maxLength, int minLength = 3, bool nullable = false, bool businessKey = false)
        {
            model.Fields.Add(new Field(name) { BusinessKey = businessKey, MaxLength = maxLength, MinLength = minLength, DataType = DataTypes.String, Nullable = nullable });
            return model;
        }
        public static Model AddFieldShort(this Model model, string name, bool nullable = false)
        {
            model.Fields.Add(new Field(name) { DataType = DataTypes.Int16, Nullable = nullable });
            return model;
        }
        public static Model AddFieldInt(this Model model, string name, bool nullable = false)
        {
            model.Fields.Add(new Field(name) { DataType = DataTypes.Int32, Nullable = nullable });
            return model;
        }
        public static Model AddFieldLong(this Model model, string name, bool nullable = false)
        {
            model.Fields.Add(new Field(name) { DataType = DataTypes.Int64, Nullable = nullable });
            return model;
        }
        public static Model AddFieldDecimal(this Model model, string name, int scale = 2, int precision = 18, bool nullable = false)
        {
            model.Fields.Add(new Field(name) { DataType = DataTypes.Decimal, Scale = scale, Precision = precision, Nullable = nullable });
            return model;
        }
        public static Model AddFieldBool(this Model model, string name)
        {
            return AddFieldBool(model, name, null, false);
        }
        public static Model AddFieldBool(this Model model, string name, string? @default, bool nullable)
        {
            return AddField(model, name, DataTypes.Bool, @default: @default, nullable: nullable);
        }
        public static Model AddField(this Model model, string name, DataTypes dataTypes, string? dataTypeTypeName = null, int? scale = null, int? precision = null, int? maxLength = null, int? minLength = null, string? @default = null, bool businessKey = false, bool nullable = false)
        {
            model.Fields.Add(new Field(name) { Default = @default, DataType = dataTypes, DataTypeTypeName = dataTypeTypeName, Scale = scale, Precision = precision, MaxLength = maxLength, MinLength = minLength, BusinessKey = businessKey, Nullable = nullable });
            return model;
        }
    }

}
