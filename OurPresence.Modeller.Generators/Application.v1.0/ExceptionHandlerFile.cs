// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace ApplicationProject
{
    internal class ExceptionHandlerFile : IGenerator
    {
        private readonly Module _module;

        public ExceptionHandlerFile(ISettings settings, Module module)
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
            sb.I(1).Al("public sealed class ExceptionHandler");
            sb.I(1).Al("{");
            sb.I(2).Al("private readonly RequestDelegate _next;");
            sb.I(2).Al("private readonly IWebHostEnvironment _env;");
            sb.B();
            sb.I(2).Al("public ExceptionHandler(RequestDelegate next, IWebHostEnvironment env)");
            sb.I(2).Al("{");
            sb.I(3).Al("_next = next;");
            sb.I(3).Al("_env = env;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public async Task Invoke(HttpContext context)");
            sb.I(2).Al("{");
            sb.I(3).Al("try");
            sb.I(3).Al("{");
            sb.I(4).Al("await _next(context);");
            sb.I(3).Al("}");
            sb.I(3).Al("catch (Exception ex)");
            sb.I(3).Al("{");
            sb.I(4).Al("await HandleException(context, ex);");
            sb.I(3).Al("}");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("private Task HandleException(HttpContext context, Exception exception)");
            sb.I(2).Al("{");
            sb.I(3).Al("string errorMessage = _env.IsProduction() ? \"Internal server error\" : \"Exception: \" + exception.Message;");
            sb.I(3).Al("Error error = Errors.General.InternalServerError(errorMessage);");
            sb.I(3).Al("Envelope envelope = Envelope.Error(error, null);");
            sb.I(3).Al("string result = JsonSerializer.Serialize(envelope);");
            sb.B();    
            sb.I(3).Al("context.Response.ContentType = \"application/json\";");
            sb.I(3).Al("context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;");
            sb.I(3).Al("return context.Response.WriteAsync(result);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File("ExceptionHandler.cs", sb.ToString());
        }
    }
}
