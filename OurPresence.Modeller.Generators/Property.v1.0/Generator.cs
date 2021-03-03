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

        public Generator(Field field, int indent = 2, bool useDataAnnotations = false, bool useBackingField = false, PropertyScope getScope = PropertyScope.@public, PropertyScope setScope = PropertyScope.@public, bool convertToLocalDates = false)
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
                sb.i(_indent).al($"private {dt} {_field.Name.Singular.ModuleVariable}");
            }

            var display = _field.Name.Singular.Display;
            if (_field.Name.ToString() != display)
            {
                if (display.EndsWith(" Id"))
                    display = display.Substring(0, display.Length - 3);
                if (_useDataAnnotations)
                    sb.i(_indent).al($"[Display(Name = \"{display}\")]");
            }
            if (_field.DataType == DataTypes.String)
            {
                if (!_field.Nullable && _useDataAnnotations)
                {
                    sb.i(_indent).al("[Required]");
                }
                if (_field.MaxLength.HasValue && _useDataAnnotations)
                {
                    sb.i(_indent).a($"[StringLength({_field.MaxLength.Value}");
                    if (_field.MinLength.HasValue)
                    {
                        sb.a($", MinimumLength={_field.MinLength.Value}");
                    }
                    sb.al(")]");
                }
            }
            else if (_field.DataType == DataTypes.DateTimeOffset && _convertToLocalDates)
            {
                sb.i(_indent).al("[DisplayFormat(DataFormatString = Constants.DateFormatVm)]");
            }
            else if (_field.DataType == DataTypes.Decimal)
            {
                if (_field.Scale.HasValue && _field.Precision.HasValue && _useDataAnnotations)
                {
                    sb.i(_indent).al($"[Range(typeof(decimal), \"{_field.Scale.Value}\", \"{_field.Precision.Value}\", ConvertValueInInvariantCulture = true)]");
                }
            }

            var scopes = SetScopes();
            sb.i(_indent).a($"{scopes.prop} {dt} {_field.Name}");
            if (_useBackingField)
            {
                sb.b();
                sb.i(_indent).al("{");
                sb.i(_indent + 1).al($"{scopes.get} get => {_field.Name.Singular.ModuleVariable};");
                sb.i(_indent + 1).al($"{scopes.set} set");
                sb.i(_indent + 1).al("{");
                sb.i(_indent + 2).al($"{_field.Name.Singular.ModuleVariable} = value;");
                sb.i(_indent + 1).al("}");
                sb.i(_indent).al("}");
            }
            else
            {
                sb.a(" {");
                sb.a($"{scopes.get} get; ");
                sb.a($"{scopes.set} set; ");
                sb.al("}");
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
                case PropertyScope.@public:
                    return 4;
                case PropertyScope.@protected:
                    return 3;
                case PropertyScope.@internal:
                    return 2;
                case PropertyScope.@private:
                    return 1;
                case PropertyScope.notAvalable:
                default:
                    return 0;
            }
        }
    }
}