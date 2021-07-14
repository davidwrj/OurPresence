// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class CommandFileGroup : IGenerator
    {
        private readonly Module _module;

        public CommandFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Commands");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Core.Commands");
            sb.Al("{");
            sb.I(1).Al("public interface ICommand: IRequest { }");
            sb.Al("}");

            group.AddFile(new File("ICommand.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Commands");
            sb.Al("{");
            sb.I(1).Al("public interface ICommandBus");
            sb.I(1).Al("{");
            sb.I(2).Al("Task Send<TCommand>(TCommand command) where TCommand : ICommand;");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("ICommandBus.cs", sb.ToString(), canOverwrite:true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Commands");
            sb.Al("{");
            sb.I(1).Al("public interface ICommandHandler<in T>: IRequestHandler<T>");
            sb.I(2).Al("where T : ICommand");
            sb.I(1).Al("{");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("ICommandHandler.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Commands");
            sb.Al("{");
            sb.I(1).Al("public class CommandBus: ICommandBus");
            sb.I(1).Al("{");
            sb.I(2).Al("private readonly IMediator _mediator;");
            sb.B();
            sb.I(2).Al("public CommandBus(IMediator mediator)");
            sb.I(2).Al("{");
            sb.I(3).Al("_mediator = mediator;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public Task Send<TCommand>(TCommand command) where TCommand : ICommand");
            sb.I(2).Al("{");
            sb.I(3).Al("return _mediator.Send(command);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("CommandBus.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
