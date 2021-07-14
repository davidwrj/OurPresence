// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class EventsFileGroup : IGenerator
    {
        private readonly Module _module;

        public EventsFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Events");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Core.Events.External");
            sb.Al("{");
            sb.I(1).Al("public interface IExternalEventProducer");
            sb.I(1).Al("{");
            sb.I(2).Al("Task Publish(IExternalEvent @event);");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("IExternalEventProducer.cs", sb.ToString(), path: "External", canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Events.External");
            sb.Al("{");
            sb.I(1).Al("public interface IExternalEventConsumer");
            sb.I(1).Al("{");
            sb.I(2).Al("Task StartAsync(CancellationToken cancellationToken);");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("IExternalEventConsumer.cs", sb.ToString(), path: "External", canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Events.External");
            sb.Al("{");
            sb.I(1).Al("public class NulloExternalEventProducer : IExternalEventProducer");
            sb.I(1).Al("{");
            sb.I(2).Al("public Task Publish(IExternalEvent @event)");
            sb.I(2).Al("{");
            sb.I(3).Al("return Task.CompletedTask;");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("NulloExternalEventProducer.cs", sb.ToString(), path: "External", canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Events");
            sb.Al("{");
            sb.I(1).Al("public interface IEvent: INotification");
            sb.I(1).Al("{ }");
            sb.Al("}");

            group.AddFile(new File("IEvent.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Events");
            sb.Al("{");
            sb.I(1).Al("public interface IExternalEvent: IEvent");
            sb.I(1).Al("{ }");
            sb.Al("}");

            group.AddFile(new File("IExternalEvent.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Queries");
            sb.Al("{");
            sb.I(1).Al("public interface IEventBus");
            sb.I(1).Al("{");
            sb.I(2).Al("Task Publish(params IEvent[] events);");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("IEventBus.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Queries");
            sb.Al("{");
            sb.I(1).Al("public interface IEventHandler<in TEvent>: INotificationHandler<TEvent>");
            sb.I(2).Al("where TEvent : IEvent");
            sb.I(1).Al("{");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("IEventHandler.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Queries");
            sb.Al("{");
            sb.I(1).Al("public class EventBus: IEventBus");
            sb.I(1).Al("{");
            sb.I(2).Al("private readonly IMediator _mediator;");
            sb.I(2).Al("private readonly IExternalEventProducer _externalEventProducer;");
            sb.B();
            sb.I(2).Al("public EventBus(IMediator mediator, IExternalEventProducer externalEventProducer)");
            sb.I(2).Al("{");
            sb.I(3).Al("_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));");
            sb.I(3).Al("_externalEventProducer = externalEventProducer ?? throw new ArgumentNullException(nameof(externalEventProducer));");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public async Task Publish(params IEvent[] events)");
            sb.I(2).Al("{");
            sb.I(3).Al("foreach (var @event in events)");
            sb.I(3).Al("{");
            sb.I(4).Al("await _mediator.Publish(@event);");
            sb.I(4).Al("if (@event is IExternalEvent externalEvent)");
            sb.I(5).Al("await _externalEventProducer.Publish(externalEvent);");
            sb.I(3).Al("}");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("EventBus.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Queries");
            sb.Al("{");
            sb.I(1).Al("public class EventTypeMapper");
            sb.I(1).Al("{");
            sb.I(2).Al("private static readonly EventTypeMapper Instance = new();");
            sb.B();
            sb.I(2).Al("private readonly ConcurrentDictionary<Type, string> TypeNameMap = new();");
            sb.I(2).Al("private readonly ConcurrentDictionary<string, Type> TypeMap = new();");
            sb.B();
            sb.I(2).Al("public static void AddCustomMap<T>(string mappedEventTypeName) => AddCustomMap(typeof(T), mappedEventTypeName);");
            sb.B();
            sb.I(2).Al("public static void AddCustomMap(Type eventType, string mappedEventTypeName)");
            sb.I(2).Al("{");
            sb.I(3).Al("Instance.TypeNameMap.AddOrUpdate(eventType, mappedEventTypeName, (_, _) => mappedEventTypeName);");
            sb.I(3).Al("Instance.TypeMap.AddOrUpdate(mappedEventTypeName, eventType, (_, _) => eventType);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static string ToName<TEventType>() => ToName(typeof(TEventType));");
            sb.B();
            sb.I(2).Al("public static string ToName(Type eventType) => Instance.TypeNameMap.GetOrAdd(eventType, (_) =>");
            sb.I(2).Al("{");
            sb.I(3).Al("var eventTypeName = eventType.FullName!.Replace(\".\", \"_\");");
            sb.B();
            sb.I(3).Al("Instance.TypeMap.AddOrUpdate(eventTypeName, eventType, (_, _) => eventType);");
            sb.B();
            sb.I(3).Al("return eventTypeName;");
            sb.I(2).Al("});");
            sb.B();
            sb.I(2).Al("public static Type ToType(string eventTypeName) => Instance.TypeMap.GetOrAdd(eventTypeName, (_) =>");
            sb.I(2).Al("{");
            sb.I(3).Al("var type = TypeProvider.GetFirstMatchingTypeFromCurrentDomainAssembly(eventTypeName.Replace(\"_\", \".\"))!;");
            sb.B();
            sb.I(3).Al("if (type == null)");
            sb.I(4).Al("throw new Exception($\"Type map for '{eventTypeName}' wasn't found!\");");
            sb.B();
            sb.I(3).Al("Instance.TypeNameMap.AddOrUpdate(type, eventTypeName, (_, _) => eventTypeName);");
            sb.B();
            sb.I(3).Al("return type;");
            sb.I(2).Al("});");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("EventTypeMapper.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Namespace}.Core.Queries");
            sb.Al("{");
            sb.I(1).Al("public class StreamNameMapper");
            sb.I(1).Al("{");
            sb.I(2).Al("private static readonly StreamNameMapper Instance = new();");
            sb.B();
            sb.I(2).Al("private readonly ConcurrentDictionary<Type, string> TypeNameMap = new();");
            sb.B();
            sb.I(2).Al("public static void AddCustomMap<TStream>(string mappedStreamName) =>");
            sb.I(3).Al("AddCustomMap(typeof(TStream), mappedStreamName);");
            sb.B();
            sb.I(2).Al("public static void AddCustomMap(Type streamType, string mappedStreamName)");
            sb.I(2).Al("{");
            sb.I(3).Al("Instance.TypeNameMap.AddOrUpdate(streamType, mappedStreamName, (_, _) => mappedStreamName);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static string ToStreamPrefix<TStream>() => ToStreamPrefix(typeof(TStream));");
            sb.B();
            sb.I(2).Al("public static string ToStreamPrefix(Type streamType) => Instance.TypeNameMap.GetOrAdd(streamType, (_) =>");
            sb.I(2).Al("{");
            sb.I(3).Al("var modulePrefix = streamType.Namespace!.Split(\".\").First();");
            sb.I(3).Al("return $\"{modulePrefix}_{streamType.Name}\";");
            sb.I(2).Al("});");
            sb.B();
            sb.I(2).Al("public static string ToStreamId<TStream>(object aggregateId, object? tenantId = null) =>");
            sb.I(3).Al("ToStreamId(typeof(TStream), aggregateId);");
            sb.B();
            sb.I(2).Al("public static string ToStreamId(Type streamType, object aggregateId, object? tenantId = null)");
            sb.I(2).Al("{");
            sb.I(3).Al("var tenantPrefix = tenantId != null ? $\"{tenantId}_\"  : \"\";");
            sb.B();
            sb.I(3).Al("return $\"{tenantPrefix}{ToStreamPrefix(streamType)}-{aggregateId}\";");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("StreamNameMapper.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
