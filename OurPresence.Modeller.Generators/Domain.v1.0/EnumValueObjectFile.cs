// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class EnumValueObjectFile : IGenerator
    {
        private readonly Module _module;

        public EnumValueObjectFile(ISettings settings, Module module)
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
            sb.I(1).Al("public abstract class EnumValueObject<TEnumeration, TId> : ValueObject");
            sb.I(3).Al("where TEnumeration : EnumValueObject<TEnumeration, TId>");
            sb.I(3).Al("where TId : struct");
            sb.I(1).Al("{");
            sb.I(2).Al("private static readonly Dictionary<TId, TEnumeration> EnumerationsById = GetEnumerations().ToDictionary(e => e.Id);");
            sb.I(2).Al("private static readonly Dictionary<string, TEnumeration> EnumerationsByName = GetEnumerations().ToDictionary(e => e.Name);");
            sb.B();
            sb.I(2).Al("protected EnumValueObject(TId id, string name)");
            sb.I(2).Al("{");
            sb.I(3).Al("if (string.IsNullOrWhiteSpace(name))");
            sb.I(3).Al("{");
            sb.I(4).Al("throw new ArgumentException(\"The name cannot be null or empty\");");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("Id = id;");
            sb.I(3).Al("Name = name;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public TId Id { get; protected set; }");
            sb.B();
            sb.I(2).Al("public string Name { get; protected set; }");
            sb.B();
            sb.I(2).Al("public static bool operator ==(EnumValueObject<TEnumeration, TId> a, TId b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return a is null ? false : a.Id.Equals(b);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator !=(EnumValueObject<TEnumeration, TId> a, TId b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return !(a == b);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator ==(TId a, EnumValueObject<TEnumeration, TId> b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return b == a;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator !=(TId a, EnumValueObject<TEnumeration, TId> b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return !(b == a);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static Maybe<TEnumeration> FromId(TId id)");
            sb.I(2).Al("{");
            sb.I(3).Al("return EnumerationsById.ContainsKey(id)");
            sb.I(4).Al("? EnumerationsById[id]");
            sb.I(4).Al(": null;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static Maybe<TEnumeration> FromName(string name)");
            sb.I(2).Al("{");
            sb.I(3).Al("return EnumerationsByName.ContainsKey(name)");
            sb.I(4).Al("? EnumerationsByName[name]");
            sb.I(4).Al(": null;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("protected override IEnumerable<object> GetEqualityComponents()");
            sb.I(2).Al("{");
            sb.I(3).Al("yield return Id;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("private static TEnumeration[] GetEnumerations()");
            sb.I(2).Al("{");
            sb.I(3).Al("var enumerationType = typeof(TEnumeration);");
            sb.B();
            sb.I(3).Al("return enumerationType");
            sb.I(4).Al(".GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)");
            sb.I(4).Al(".Where(info => info.FieldType == typeof(TEnumeration))");
            sb.I(4).Al(".Select(info => (TEnumeration)info.GetValue(null))");
            sb.I(4).Al(".ToArray();");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.B();
            sb.I(1).Al("public abstract class EnumValueObject<TEnumeration> : ValueObject");
            sb.I(2).Al("where TEnumeration : EnumValueObject<TEnumeration>");
            sb.I(1).Al("{");
            sb.I(2).Al("private static readonly Dictionary<string, TEnumeration> Enumerations = GetEnumerations().ToDictionary(e => e.Id);");
            sb.B();
            sb.I(2).Al("protected EnumValueObject(string id)");
            sb.I(2).Al("{");
            sb.I(3).Al("if (string.IsNullOrWhiteSpace(id))");
            sb.I(3).Al("{");
            sb.I(4).Al("throw new ArgumentException(\"The enum key cannot be null or empty\");");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("Id = id;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static IEnumerable<TEnumeration> All = Enumerations.Values;");
            sb.B();
            sb.I(2).Al("public virtual string Id { get; protected set; }");
            sb.B();
            sb.I(2).Al("public static bool operator ==(EnumValueObject<TEnumeration> a, string b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return a is null && b is null ? true : a is null || b is null ? false : a.Id.Equals(b);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator !=(EnumValueObject<TEnumeration> a, string b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return !(a == b);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator ==(string a, EnumValueObject<TEnumeration> b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return b == a;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool operator !=(string a, EnumValueObject<TEnumeration> b)");
            sb.I(2).Al("{");
            sb.I(3).Al("return !(b == a);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static Maybe<TEnumeration> FromId(string id)");
            sb.I(2).Al("{");
            sb.I(3).Al("return Enumerations.ContainsKey(id)");
            sb.I(4).Al("? Enumerations[id]");
            sb.I(4).Al(": null;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static bool Is(string possibleKey) => All.Select(e => e.Id).Contains(possibleKey);");
            sb.B();
            sb.I(2).Al("public override string ToString() => Id;");
            sb.B();
            sb.I(2).Al("protected override IEnumerable<object> GetEqualityComponents()");
            sb.I(2).Al("{");
            sb.I(3).Al("yield return Id;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("private static TEnumeration[] GetEnumerations()");
            sb.I(2).Al("{");
            sb.I(3).Al("var enumerationType = typeof(TEnumeration);");
            sb.B();
            sb.I(3).Al("return enumerationType");
            sb.I(4).Al(".GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)");
            sb.I(4).Al(".Where(info => info.FieldType == typeof(TEnumeration))");
            sb.I(4).Al(".Select(info => (TEnumeration)info.GetValue(null))");
            sb.I(4).Al(".ToArray();");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");


            return new File("EnumValueObject.cs", sb.ToString());
        }
    }
}
