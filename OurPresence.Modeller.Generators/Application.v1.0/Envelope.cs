// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace ApplicationProject
{
    internal class EnvelopeFile : IGenerator
    {
        private readonly Module _module;

        public EnvelopeFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Application");
            sb.Al("{");
            sb.I(1).Al("public class Envelope");
            sb.I(1).Al("{");

            sb.I(2).Al("public object Result { get; }");
            sb.I(2).Al("public string ErrorCode { get; }");
            sb.I(2).Al("public string ErrorMessage { get; }");
            sb.I(2).Al("public string InvalidField { get; }");
            sb.I(2).Al("public DateTime TimeGenerated { get; }");
            sb.B();
            sb.I(2).Al("private Envelope(object result, Error error, string invalidField)");
            sb.I(2).Al("{");
            sb.I(2).Al("    Result = result;");
            sb.I(2).Al("    ErrorCode = error?.Code;");
            sb.I(2).Al("    ErrorMessage = error?.Message;");
            sb.I(2).Al("    InvalidField = invalidField;");
            sb.I(2).Al("    TimeGenerated = DateTime.UtcNow;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static Envelope Ok(object result = null)");
            sb.I(2).Al("{");
            sb.I(2).Al("    return new Envelope(result, null, null);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static Envelope Error(Error error, string invalidField)");
            sb.I(2).Al("{");
            sb.I(2).Al("    return new Envelope(null, error, invalidField);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File("Envelop.cs", sb.ToString());
        }
    }
}
