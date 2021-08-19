// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class IdsFileGroup : IGenerator
    {
        private readonly Module _module;

        public IdsFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Ids");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Company}.{_module.Project}.Core.Ids");
            sb.Al("{");
            sb.I(1).Al("public interface IIdGenerator");
            sb.I(1).Al("{");
            sb.I(2).Al("Guid New();");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("IIdGenerator.cs", sb.ToString(), canOverwrite: true));

            sb.Clear();
            sb.Al($"namespace {_module.Company}.{_module.Project}.Core.Ids");
            sb.Al("{");
            sb.I(1).Al("public class NulloIdGenerator : IIdGenerator");
            sb.I(1).Al("{");
            sb.I(2).Al("public Guid New() => Guid.NewGuid();");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("NulloIdGenerator.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
