using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class ValueObjectFile : IGenerator
    {
        private readonly Module _module;

        public ValueObjectFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Common");
            sb.Al("{");
            sb.I(1).Al("[Serializable]");
            sb.I(1).Al("public abstract class ValueObject<T>");
            sb.I(3).Al("where T : ValueObject<T>");
            sb.I(1).Al("{");
            sb.I(2).Al("private int? _cachedHashCode;");
            sb.B();
            sb.I(2).Al("public override bool Equals(object obj)");
            sb.I(2).Al("{");
            sb.I(3).Al("var valueObject = obj as T;");
            sb.B();
            sb.I(3).Al("return valueObject is null");
            sb.I(4).Al("? false");
            sb.I(4).Al(": ValueObject.GetUnproxiedType(this) != ValueObject.GetUnproxiedType(obj) ? false : EqualsCore(valueObject);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("protected abstract bool EqualsCore(T other);");
            sb.B();
            sb.I(2).Al("public override int GetHashCode()");
            sb.I(2).Al("{");
            sb.I(3).Al("if (!_cachedHashCode.HasValue)");
            sb.I(3).Al("{");
            sb.I(4).Al("_cachedHashCode = GetHashCodeCore();");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("return _cachedHashCode.Value;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("protected abstract int GetHashCodeCore();");
            sb.B();
            sb.I(2).Al("public static bool operator ==(ValueObject<T> a, ValueObject<T> b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return a is null && b is null ? true : a is null || b is null ? false : a.Equals(b);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator !=(ValueObject<T> a, ValueObject<T> b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return !(a == b);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.B();
            sb.I(1).Al("public abstract class ValueObject : IComparable, IComparable<ValueObject>");
            sb.I(1).Al("{");
            sb.I(2).Al("private int? _cachedHashCode;");
            sb.B();
            sb.I(2).Al("protected abstract IEnumerable<object> GetEqualityComponents();");
            sb.B();
            sb.I(2).Al("public override bool Equals(object obj)");
            sb.I(2).Al("{");
            sb.I(3).Al("if (obj == null)");
            sb.I(3).Al("{");
            sb.I(4).Al("return false;");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("if (GetUnproxiedType(this) != GetUnproxiedType(obj))");
            sb.I(3).Al("{");
            sb.I(4).Al("return false;");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("var valueObject = (ValueObject)obj;");
            sb.B();
            sb.I(3).Al("return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public override int GetHashCode()");
            sb.I(2).Al("{");
            sb.I(3).Al("if (!_cachedHashCode.HasValue)");
            sb.I(3).Al("{");
            sb.I(4).Al("_cachedHashCode = GetEqualityComponents()");
            sb.I(5).Al(".Aggregate(1, (current, obj) =>");
            sb.I(5).Al("{");
            sb.I(6).Al("unchecked");
            sb.I(6).Al("{");
            sb.I(7).Al("return current * 23 + (obj?.GetHashCode() ?? 0);");
            sb.I(6).Al("}");
            sb.I(5).Al("});");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("return _cachedHashCode.Value;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public virtual int CompareTo(object obj)");
            sb.I(2).Al("{");
            sb.I(3).Al("var thisType = GetUnproxiedType(this);");
            sb.I(3).Al("var otherType = GetUnproxiedType(obj);");
            sb.B();
            sb.I(3).Al("if (thisType != otherType)");
            sb.I(3).Al("{");
            sb.I(4).Al("return string.Compare(thisType.ToString(), otherType.ToString(), StringComparison.Ordinal);");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("var other = (ValueObject)obj;");
            sb.B();
            sb.I(3).Al("var components = GetEqualityComponents().ToArray();");
            sb.I(3).Al("var otherComponents = other.GetEqualityComponents().ToArray();");
            sb.B();
            sb.I(3).Al("for (var i = 0; i < components.Length; i++)");
            sb.I(3).Al("{");
            sb.I(4).Al("var comparison = CompareComponents(components[i], otherComponents[i]);");
            sb.I(4).Al("if (comparison != 0)");
            sb.I(4).Al("{");
            sb.I(5).Al("return comparison;");
            sb.I(4).Al("}");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("return 0;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("private int CompareComponents(object object1, object object2)");
            sb.I(2).Al("{");
            sb.I(3).Al("if (object1 is null && object2 is null)");
            sb.I(3).Al("{");
            sb.I(4).Al("return 0;");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("if (object1 is null)");
            sb.I(3).Al("{");
            sb.I(4).Al("return -1;");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("return object2 is null");
            sb.I(4).Al("? 1");
            sb.I(4).Al(": object1 is IComparable comparable1 && object2 is IComparable comparable2");
            sb.I(4).Al("? comparable1.CompareTo(comparable2)");
            sb.I(4).Al(": object1.Equals(object2) ? 0 : -1;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public virtual int CompareTo(ValueObject other)");
            sb.I(2).Al("{");
            sb.I(3).Al("return CompareTo(other as object);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator ==(ValueObject a, ValueObject b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return a is null && b is null ? true : a is null || b is null ? false : a.Equals(b);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator !=(ValueObject a, ValueObject b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return !(a == b);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("internal static Type GetUnproxiedType(object obj)");
            sb.I(2).Al("{");
            sb.I(3).Al("const string EFCoreProxyPrefix = \"Castle.Proxies.\";");
            sb.I(3).Al("const string NHibernateProxyPostfix = \"Proxy\";");
            sb.B();
            sb.I(3).Al("var type = obj.GetType();");
            sb.I(3).Al("var typeString = type.ToString();");
            sb.B();
            sb.I(3).Al("return typeString.Contains(EFCoreProxyPrefix) || typeString.EndsWith(NHibernateProxyPostfix) ? type.BaseType : type;");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File("ValueObject.cs", sb.ToString());
        }
    }
}
