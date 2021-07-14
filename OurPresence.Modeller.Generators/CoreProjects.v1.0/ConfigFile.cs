// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class ConfigFile : IGenerator
    {
        private readonly Module _module;

        public ConfigFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();

            sb.Al($"namespace {_module.Namespace}.Core.Exceptions");
            sb.Al("{");
            sb.I(1).Al("public static class Config");
            sb.I(1).Al("{");
            sb.I(2).Al("public static IServiceCollection AddCoreServices(this IServiceCollection services)");
            sb.I(2).Al("{");
            sb.I(3).Al("services.AddMediatR()");
            sb.I(4).Al(".AddScoped<ICommandBus, CommandBus>()");
            sb.I(4).Al(".AddScoped<IQueryBus, QueryBus>();");
            sb.I(1).Al("");
            sb.I(3).Al("services.TryAddScoped<IEventBus, EventBus>();");
            sb.I(3).Al("services.TryAddScoped<IExternalEventProducer, NulloExternalEventProducer>();");
            sb.I(3).Al("services.TryAddScoped<IExternalCommandBus, ExternalCommandBus>();");
            sb.I(1).Al("");
            sb.I(3).Al("services.TryAddScoped<IIdGenerator, NulloIdGenerator>();");
            sb.I(1).Al("");
            sb.I(3).Al("return services;");
            sb.I(2).Al("}");
            sb.I(1).Al("");
            sb.I(2).Al("private static IServiceCollection AddMediatR(this IServiceCollection services)");
            sb.I(2).Al("{");
            sb.I(3).Al("return services.AddScoped<IMediator, Mediator>()");
            sb.I(4).Al(".AddTransient<ServiceFactory>(sp => sp.GetRequiredService!);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File("Config.cs", sb.ToString(), canOverwrite: true);
        }
    }
}
