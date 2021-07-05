// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class SimpleValueObjectFile : IGenerator
    {
        private readonly Module _module;

        public SimpleValueObjectFile(ISettings settings, Module module)
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
            sb.I(1).Al("public abstract class SimpleValueObject<T> : ValueObject");
            sb.I(1).Al("{");
            sb.I(2).Al("public T Value { get; }");
            sb.B();
            sb.I(2).Al("protected SimpleValueObject(T value)");
            sb.I(2).Al("{");
            sb.I(3).Al("Value = value;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("protected override IEnumerable<object> GetEqualityComponents()");
            sb.I(2).Al("{");
            sb.I(3).Al("yield return Value;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public override string? ToString()");
            sb.I(2).Al("{");
            sb.I(3).Al("return Value?.ToString();");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static implicit operator T(SimpleValueObject<T> valueObject)");
            sb.I(2).Al("{");
            sb.I(3).Al("return valueObject == null ? default : valueObject.Value;");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File("SimpleValueObject.cs", sb.ToString());
        }
    }
}
