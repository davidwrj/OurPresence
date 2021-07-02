﻿using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace ApplicationProject
{
    internal class ApplicationControllerFile : IGenerator
    {
        private readonly Module _module;

        public ApplicationControllerFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al($"using {_module.Namespace}.Common;");
            sb.Al($"using {_module.Namespace}.Common.EntityTypes;");
            sb.B();
            sb.Al($"namespace {_module.Namespace}.Controllers");
            sb.Al("{");
            sb.I(2).Al("[ApiController]");
            sb.I(2).Al("public abstract class ApplicationController : ControllerBase");
            sb.I(2).Al("{");
            sb.I(3).Al("protected new IActionResult Ok(object result = null)");
            sb.I(3).Al("{");
            sb.I(4).Al("return new EnvelopeResult(Envelope.Ok(result), HttpStatusCode.OK);");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("protected IActionResult NotFound(Error error, string invalidField = null)");
            sb.I(3).Al("{");
            sb.I(4).Al("return new EnvelopeResult(Envelope.Error(error, invalidField), HttpStatusCode.NotFound);");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("protected IActionResult Error(Error error, string invalidField = null)");
            sb.I(3).Al("{");
            sb.I(4).Al("return new EnvelopeResult(Envelope.Error(error, invalidField), HttpStatusCode.BadRequest);");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("protected IActionResult FromResult(Result result)");
            sb.I(3).Al("{");
            sb.I(4).Al("return result.IsSuccess ? Ok() : Error(Errors.General.InternalServerError(GetType().Name), result.Error);");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("protected IActionResult FromResult<T>(Result<T> result)");
            sb.I(3).Al("{");
            sb.I(4).Al("return result.IsSuccess ? Ok(result.Value) : Error(Errors.General.InternalServerError(GetType().Name), result.Error);");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("protected IActionResult FromResult<T>(Result<T, Error> result)");
            sb.I(3).Al("{");
            sb.I(4).Al("return result.IsSuccess ? Ok(result.Value) : Error(result.Error);");
            sb.I(3).Al("}");
            sb.I(2).Al("}");
            sb.Al("}");

            return new File("ApplicationController.cs", sb.ToString());
        }
    }
}
