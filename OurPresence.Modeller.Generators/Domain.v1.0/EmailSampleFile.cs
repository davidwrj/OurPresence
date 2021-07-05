// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class EmailSampleFile : IGenerator
    {
        private readonly Module _module;

        public EmailSampleFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Common.Domain");
            sb.Al("{");
            sb.I(1).Al("public class Email : ValueObject");
            sb.I(1).Al("{");
            sb.I(2).Al("public string Value { get; }");
            sb.B();
            sb.I(2).Al("private Email(string value)");
            sb.I(2).Al("{");
            sb.I(3).Al("Value = value;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static Result<Email, Error> Create(string input)");
            sb.I(2).Al("{");
            sb.I(3).Al("if (string.IsNullOrWhiteSpace(input))");
            sb.I(3).Al("{");
            sb.I(3).Al("    return Errors.General.ValueIsRequired();");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("var email = input.Trim();");
            sb.B();
            sb.I(3).Al("return email.Length > 500");
            sb.I(4).Al("? Errors.General.InvalidLength()");
            sb.I(4).Al(": Regex.IsMatch(email, @\"^(.+)@(.+)$\") == false ? Errors.General.ValueIsInvalid() : new Email(email);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("protected override IEnumerable<object> GetEqualityComponents()");
            sb.I(2).Al("{");
            sb.I(3).Al("yield return Value;");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File("Email.cs", sb.ToString(), path: "Domain");
        }
    }
}
