// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class BaseAsyncEndpointGroup : IGenerator
    {
        private readonly Module _module;

        public BaseAsyncEndpointGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("ApiEndpoints");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Company}.{_module.Project}.Core.ApiEndpoints");
            sb.Al("{");
            sb.I(1).Al("public static class BaseAsyncEndpoint");
            sb.I(1).Al("{");
            sb.I(2).Al("public static class WithRequest<TRequest>");
            sb.I(2).Al("{");
            sb.I(3).Al("public abstract class WithResponse<TResponse> : BaseEndpointAsync");
            sb.I(3).Al("{");
            sb.I(4).Al("public abstract Task<ActionResult<TResponse>> HandleAsync(");
            sb.I(5).Al("TRequest request,");
            sb.I(5).Al("CancellationToken cancellationToken = default");
            sb.I(4).Al(");");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("public abstract class WithoutResponse : BaseEndpointAsync");
            sb.I(3).Al("{");
            sb.I(4).Al("public abstract Task<ActionResult> HandleAsync(");
            sb.I(5).Al("TRequest request,");
            sb.I(5).Al("CancellationToken cancellationToken = default");
            sb.I(4).Al(");");
            sb.I(3).Al("}");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static class WithoutRequest");
            sb.I(2).Al("{");
            sb.I(3).Al("public abstract class WithResponse<TResponse> : BaseEndpointAsync");
            sb.I(3).Al("{");
            sb.I(4).Al("public abstract Task<ActionResult<TResponse>> HandleAsync(");
            sb.I(5).Al("CancellationToken cancellationToken = default");
            sb.I(4).Al(");");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("public abstract class WithoutResponse : BaseEndpointAsync");
            sb.I(3).Al("{");
            sb.I(4).Al("public abstract Task<ActionResult> HandleAsync(");
            sb.I(5).Al("CancellationToken cancellationToken = default");
            sb.I(4).Al(");");
            sb.I(3).Al("}");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.B();
            sb.I(1).Al("[ApiController]");
            sb.I(1).Al("public abstract class BaseEndpointAsync : ControllerBase");
            sb.I(1).Al("{");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("BaseAsyncEndpoint.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Company}.{_module.Project}.Core.ApiEndpoints");
            sb.Al("{");
            sb.I(1).Al("/// <summary>");
            sb.I(1).Al("/// A base class for an endpoint that accepts parameters.");
            sb.I(1).Al("/// </summary>");
            sb.I(1).Al("/// <typeparam name=\"TRequest\"></typeparam>");
            sb.I(1).Al("/// <typeparam name=\"TResponse\"></typeparam>");
            sb.I(1).Al("public static class BaseEndpoint");
            sb.I(1).Al("{");
            sb.I(2).Al("public static class WithRequest<TRequest>");
            sb.I(2).Al("{");
            sb.I(3).Al("public abstract class WithResponse<TResponse> : BaseEndpointSync");
            sb.I(3).Al("{");
            sb.I(4).Al("public abstract ActionResult<TResponse> Handle(TRequest request);");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("public abstract class WithoutResponse : BaseEndpointSync");
            sb.I(3).Al("{");
            sb.I(4).Al("public abstract ActionResult Handle(TRequest request);");
            sb.I(3).Al("}");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static class WithoutRequest");
            sb.I(2).Al("{");
            sb.I(3).Al("public abstract class WithResponse<TResponse> : BaseEndpointSync");
            sb.I(3).Al("{");
            sb.I(4).Al("public abstract ActionResult<TResponse> Handle();");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("public abstract class WithoutResponse : BaseEndpointSync");
            sb.I(3).Al("{");
            sb.I(4).Al("public abstract ActionResult Handle();");
            sb.I(3).Al("}");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.B();
            sb.I(1).Al("/// <summary>");
            sb.I(1).Al("/// A base class for all synchronous endpoints.");
            sb.I(1).Al("/// </summary>");
            sb.I(1).Al("[ApiController]");
            sb.I(1).Al("public abstract class BaseEndpointSync : ControllerBase");
            sb.I(1).Al("{");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("BaseEndpoint.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
