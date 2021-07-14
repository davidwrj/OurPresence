// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class AggregateFileGroup : IGenerator
    {
        private readonly Module _module;

        public AggregateFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Aggregates");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Core.Aggregates");
            sb.Al("{");
            sb.I(1).Al("public abstract class Aggregate: Aggregate<Guid>, IAggregate");
            sb.I(1).Al("{ }");
            sb.B();
            sb.I(1).Al("public abstract class Aggregate<T>: IAggregate<T> where T : notnull");
            sb.I(1).Al("{");
            sb.B();
            sb.I(2).Al("public T Id { get; protected set; } = default!;");
            sb.B();
            sb.I(2).Al("public int Version { get; protected set; }");
            sb.B();
            sb.I(2).Al("[NonSerialized] private readonly Queue<IEvent> uncommittedEvents = new Queue<IEvent>();");
            sb.B();
            sb.I(2).Al("public virtual void When(object @event) { }");
            sb.B();
            sb.I(2).Al("public IEvent[] DequeueUncommittedEvents()");
            sb.I(2).Al("{");
            sb.I(3).Al("var dequeuedEvents = uncommittedEvents.ToArray();");
            sb.B();
            sb.I(3).Al("uncommittedEvents.Clear();");
            sb.B();
            sb.I(3).Al("return dequeuedEvents;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("protected void Enqueue(IEvent @event)");
            sb.I(2).Al("{");
            sb.I(3).Al("uncommittedEvents.Enqueue(@event);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("Aggregate.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Aggregates");
            sb.Al("{");
            sb.I(1).Al("public interface IAggregate: IAggregate<Guid>");
            sb.B();
            sb.I(1).Al("{ }");
            sb.B();
            sb.I(1).Al("public interface IAggregate<out T>: IProjection");
            sb.I(1).Al("{");
            sb.I(2).Al("T Id { get; }");
            sb.I(2).Al("int Version { get; }");
            sb.B();
            sb.I(2).Al("IEvent[] DequeueUncommittedEvents();");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("IAggregate.cs", sb.ToString(), canOverwrite:true));

            return group;
        }
    }
}
