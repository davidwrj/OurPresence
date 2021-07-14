// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class QueryFileGroup : IGenerator
    {
        private readonly Module _module;

        public QueryFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Queries");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Core.Queries");
            sb.Al("{");
            sb.I(1).Al("public interface IQuery<out TResponse>: IRequest<TResponse>");
            sb.I(1).Al("{ }");
            sb.Al("}");

            group.AddFile(new File("IQuery.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Queries");
            sb.Al("{");
            sb.I(1).Al("public interface IQueryBus");
            sb.I(1).Al("{");
            sb.I(2).Al("Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>;");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("IQueryBus.cs", sb.ToString(), canOverwrite:true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Queries");
            sb.Al("{");
            sb.I(1).Al("public interface IQueryHandler<in TQuery, TResponse>: IRequestHandler<TQuery, TResponse>");
            sb.I(2).Al("where TQuery : IQuery<TResponse>");
            sb.I(1).Al("{");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("IQueryHandler.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Queries");
            sb.Al("{");
            sb.I(1).Al("public class QueryBus: IQueryBus");
            sb.I(1).Al("{");
            sb.I(2).Al("private readonly IMediator _mediator;");
            sb.B();
            sb.I(2).Al("public QueryBus(IMediator mediator)");
            sb.I(2).Al("{");
            sb.I(3).Al("_mediator = mediator;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>");
            sb.I(2).Al("{");
            sb.I(3).Al("return _mediator.Send(query);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("QueryBus.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
