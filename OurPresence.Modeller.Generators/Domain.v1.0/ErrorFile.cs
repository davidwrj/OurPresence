// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class ErrorFile : IGenerator
    {
        private readonly Module _module;

        public ErrorFile(ISettings settings, Module module)
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
            sb.I(1).Al("public sealed class Error : ValueObject");
            sb.I(1).Al("{");
            sb.I(2).Al("private const string Separator = \"||\";");
            sb.B();
            sb.I(2).Al("public string Code { get; }");
            sb.I(2).Al("public string Message { get; }");
            sb.B();
            sb.I(2).Al("internal Error(string code, string message)");
            sb.I(2).Al("{");
            sb.I(3).Al("Code = code;");
            sb.I(3).Al("Message = message;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("protected override IEnumerable<object> GetEqualityComponents()");
            sb.I(2).Al("{");
            sb.I(3).Al("yield return Code;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public string Serialize()");
            sb.I(2).Al("{");
            sb.I(3).Al("return $\"{Code}{Separator}{Message}\";");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static Error Deserialize(string serialized)");
            sb.I(2).Al("{");
            sb.I(3).Al("if (serialized == \"A non-empty request body is required.\")");
            sb.I(3).Al("{");
            sb.I(4).Al("return Errors.General.ValueIsRequired();");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("var data = serialized.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);");
            sb.B();
            sb.I(3).Al("return data.Length < 2 ? throw new Exception($\"Invalid error serialization: '{serialized}'\") : new Error(data[0], data[1]);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.B();
            sb.I(1).Al("public static class Errors");
            sb.I(1).Al("{");
            sb.I(2).Al("public static class Vehicle");
            sb.I(2).Al("{");
            sb.I(3).Al("public static Error InvalidSearch(string error)");
            sb.I(3).Al("{");
            sb.I(4).Al("return new Error(\"invalid.vehicle.search\", $\"Invalid vehicle search. {error}\");");
            sb.I(3).Al("}");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static class General");
            sb.I(2).Al("{");
            sb.I(3).Al("public static Error NotFound(long? id = null)");
            sb.I(3).Al("{");
            sb.I(4).Al("var forId = id == null ? \"\" : $\" for Id '{id}'\";");
            sb.I(4).Al("return new Error(\"record.not.found\", $\"Record not found{forId}\");");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("public static Error ValueIsInvalid() =>");
            sb.I(4).Al("new Error(\"value.is.invalid\", \"Value is invalid\");");
            sb.B();
            sb.I(3).Al("public static Error ValueIsRequired() =>");
            sb.I(4).Al("new Error(\"value.is.required\", \"Value is required\");");
            sb.B();
            sb.I(3).Al("public static Error InvalidLength(string name = null)");
            sb.I(3).Al("{");
            sb.I(4).Al("var label = name == null ? \" \" : \" \" + name + \" \";");
            sb.I(4).Al("return new Error(\"invalid.string.length\", $\"Invalid{label}length\");");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("public static Error CollectionIsTooSmall(int min, int current)");
            sb.I(3).Al("{");
            sb.I(4).Al("return new Error(");
            sb.I(5).Al("\"collection.is.too.small\",");
            sb.I(5).Al("$\"The collection must contain {min} items or more. It contains {current} items.\");");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("public static Error CollectionIsTooLarge(int max, int current)");
            sb.I(3).Al("{");
            sb.I(4).Al("return new Error(");
            sb.I(5).Al("\"collection.is.too.large\",");
            sb.I(5).Al("$\"The collection must contain {max} items or more. It contains {current} items.\");");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("public static Error InternalServerError(string message)");
            sb.I(3).Al("{");
            sb.I(4).Al("return new Error(\"internal.server.error\", message);");
            sb.I(3).Al("}");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File("Error.cs", sb.ToString());
        }
    }
}
