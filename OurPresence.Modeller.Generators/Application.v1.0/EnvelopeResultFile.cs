using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace ApplicationProject
{
    internal class EnvelopeResultFile : IGenerator
    {
        private readonly Module _module;

        public EnvelopeResultFile(ISettings settings, Module module)
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
            sb.I(1).Al("public sealed class EnvelopeResult : IActionResult");
            sb.I(1).Al("{");
            sb.I(2).Al("private readonly Envelope _envelope;");
            sb.I(2).Al("private readonly int _statusCode;");
            sb.B();
            sb.I(2).Al("public EnvelopeResult(Envelope envelope, HttpStatusCode statusCode)");
            sb.I(2).Al("{");
            sb.I(3).Al("_statusCode = (int)statusCode;");
            sb.I(3).Al("_envelope = envelope;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public Task ExecuteResultAsync(ActionContext context)");
            sb.I(2).Al("{");
            sb.I(3).Al("var objectResult = new ObjectResult(_envelope)");
            sb.I(3).Al("{");
            sb.I(4).Al("StatusCode = _statusCode");
            sb.I(3).Al("};");
            sb.I(3).Al("return objectResult.ExecuteResultAsync(context);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File("EnvelopResult.cs", sb.ToString());
        }
    }
}
