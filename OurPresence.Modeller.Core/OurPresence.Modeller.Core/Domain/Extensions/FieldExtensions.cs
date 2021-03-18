namespace OurPresence.Modeller.Domain.Extensions
{
    public static class FieldExtensions
    {
        public static string GetDataType(this Field field, bool showNullable = true, bool guidNullable = false)
        {
            string type;
            switch (field.DataType)
            {
                case DataTypes.Bool:
                    type = "bool";
                    break;

                case DataTypes.Time:
                    type = "TimeSpan";
                    break;

                case DataTypes.Date:
                case DataTypes.DateTime:
                case DataTypes.DateTimeOffset:
                    type = field.DataType.ToString();
                    break;
                case DataTypes.Decimal:
                    type = "decimal";
                    break;
                case DataTypes.UniqueIdentifier:
                    type = "Guid";
                    break;
                case DataTypes.Object:
                    type = string.IsNullOrWhiteSpace(field.DataTypeTypeName) ? "object" : field.DataTypeTypeName;
                    showNullable = false;
                    break;
                case DataTypes.Byte:
                    type = "byte";
                    break;
                case DataTypes.Int16:
                    type = "short";
                    break;
                case DataTypes.Int32:
                    type = "int";
                    break;
                case DataTypes.Int64:
                    type = "long";
                    break;
                case DataTypes.String:
                default:
                    type = "string";
                    break;
            }
            if (showNullable && (field.Nullable || (type == "Guid" && guidNullable)))
                type += "?";
            return type;
        }

        public static bool IsForeignKey(this Field field, Model model, Module module)
        {
            return module.IsForeignKey(model, new[] { field.Name.Value });
        }
    }
}
