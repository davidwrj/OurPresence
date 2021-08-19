// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class ExceptionsFileGroup : IGenerator
    {
        private readonly Module _module;

        public ExceptionsFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Exceptions");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Company}.{_module.Project}.Core.Exceptions");
            sb.Al("{");
            sb.I(1).Al("public class AggregateNotFoundException : Exception");
            sb.I(1).Al("{");
            sb.I(2).Al("public AggregateNotFoundException(string typeName, Guid id): base($\"{typeName} with id '{id}' was not found\")");
            sb.I(2).Al("{ }");
            sb.B();
            sb.I(2).Al("public static AggregateNotFoundException For<T>(Guid id)");
            sb.I(2).Al("{");
            sb.I(3).Al("return new AggregateNotFoundException(typeof(T).Name, id);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("AggregateNotFoundException.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
