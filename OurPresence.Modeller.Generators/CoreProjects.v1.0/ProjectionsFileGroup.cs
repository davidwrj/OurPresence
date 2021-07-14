// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class ProjectionsFileGroup : IGenerator
    {
        private readonly Module _module;

        public ProjectionsFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Projections");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Core.Projections");
            sb.Al("{");
            sb.I(1).Al("public interface IProjection");
            sb.I(1).Al("{");
            sb.I(2).Al("void When(object @event);");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("IProjection.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
