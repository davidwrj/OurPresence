using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace Property
{
    public class Generator : IGenerator
    {
        private readonly Field _field;
        private readonly int _indent;
        private readonly bool _useDataAnnotations;
        private readonly bool _useBackingField;
        private readonly PropertyScope _getScope;
        private readonly PropertyScope _setScope;
        private readonly bool _convertToLocalDates;

        public Generator(Field field, int indent = 2, bool useDataAnnotations = false, bool useBackingField = false, PropertyScope getScope = PropertyScope.Public, PropertyScope setScope = PropertyScope.Public, bool convertToLocalDates = false)
        {
            _field = field ?? throw new ArgumentNullException(nameof(field));
            if (indent < 1)
            {
                indent = 1;
            }
            _indent = indent;
            _useDataAnnotations = useDataAnnotations;
            _useBackingField = useBackingField;
            _getScope = getScope;
            _setScope = setScope;
            _convertToLocalDates = convertToLocalDates;
        }

        public bool ShowNullable { get; set; } = true;

        public bool GuidNullable { get; set; } = false;

        public ISettings Settings => throw new NotImplementedException();

        public IOutput Create()
        {
            string dt;
            if (_field.DataType == DataTypes.DateTimeOffset && _convertToLocalDates)
            {
                dt = "DateTime";
                if (_field.Nullable)
                    dt += "?";
            }
            else
            {
                dt = _field.GetDataType(ShowNullable, GuidNullable);
            }

            if (ShowNullable && _field.DataType == DataTypes.String && _field.Nullable && !dt.EndsWith("?"))
            {
                dt += "?";
            }

            var sb = new StringBuilder();
            if (_useBackingField)
            {
                sb.I(_indent).Al($"private {dt} {_field.Name.Singular.ModuleVariable}");
            }

            var display = _field.Name.Singular.Display;
            if (_field.Name.ToString() != display)
            {
                if (display.EndsWith(" Id"))
                    display = display.Substring(0, display.Length - 3);
                if (_useDataAnnotations)
                    sb.I(_indent).Al($"[Display(Name = \"{display}\")]");
            }
            if (_field.DataType == DataTypes.String)
            {
                if (!_field.Nullable && _useDataAnnotations)
                {
                    sb.I(_indent).Al("[Required]");
                }
                if (_field.MaxLength.HasValue && _useDataAnnotations)
                {
                    sb.I(_indent).A($"[StringLength({_field.MaxLength.Value}");
                    if (_field.MinLength.HasValue)
                    {
                        sb.A($", MinimumLength={_field.MinLength.Value}");
                    }
                    sb.Al(")]");
                }
            }
            else if (_field.DataType == DataTypes.DateTimeOffset && _convertToLocalDates)
            {
                sb.I(_indent).Al("[DisplayFormat(DataFormatString = Constants.DateFormatVm)]");
            }
            else if (_field.DataType == DataTypes.Decimal)
            {
                if (_field.Scale.HasValue && _field.Precision.HasValue && _useDataAnnotations)
                {
                    sb.I(_indent).Al($"[Range(typeof(decimal), \"{_field.Scale.Value}\", \"{_field.Precision.Value}\", ConvertValueInInvariantCulture = true)]");
                }
            }

            var scopes = SetScopes();
            sb.I(_indent).A($"{scopes.prop} {dt} {_field.Name}");
            if (_useBackingField)
            {
                sb.B();
                sb.I(_indent).Al("{");
                sb.I(_indent + 1).Al($"{scopes.get} get => {_field.Name.Singular.ModuleVariable};");
                sb.I(_indent + 1).Al($"{scopes.set} set");
                sb.I(_indent + 1).Al("{");
                sb.I(_indent + 2).Al($"{_field.Name.Singular.ModuleVariable} = value;");
                sb.I(_indent + 1).Al("}");
                sb.I(_indent).Al("}");
            }
            else
            {
                sb.A(" {");
                sb.A($"{scopes.get} get; ");
                sb.A($"{scopes.set} set; ");
                sb.Al("}");
            }
            return new Snippet(sb.ToString());
        }

        private (string prop, string set, string get) SetScopes()
        {
            if (_setScope == _getScope)
                return (_getScope.ToString(), "", "");

            var g = GetValue(_getScope);
            var s = GetValue(_setScope);
            var m = Math.Max(g, s);
            var p = GetScope(m);
            var other = Math.Min(g, s);
            return other == g ? (p, "", GetScope(g)) : (p, GetScope(s), "");
        }

        private string GetScope(int value)
        {
            switch (value)
            {
                case 4: return "public";
                case 3: return "protected";
                case 2: return "internal";
                case 1: return "private";
                default: return string.Empty;
            }
        }

        private int GetValue(PropertyScope scope)
        {
            switch (scope)
            {
                case PropertyScope.Public:
                    return 4;
                case PropertyScope.Protected:
                    return 3;
                case PropertyScope.Internal:
                    return 2;
                case PropertyScope.Private:
                    return 1;
                case PropertyScope.NotAvailable:
                default:
                    return 0;
            }
        }
    }
}