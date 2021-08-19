// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class ReflectionFileGroup : IGenerator
    {
        private readonly Module _module;

        public ReflectionFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Reflections");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Company}.{_module.Project}.Core.Reflections");
            sb.Al("{");
            sb.I(1).Al("public static class TypeProvider");
            sb.I(1).Al("{");
            sb.I(2).Al("public static Type? GetTypeFromAnyReferencingAssembly(string typeName)");
            sb.I(2).Al("{");
            sb.I(3).Al("var referencedAssemblies = System.Reflection.Assembly.GetEntryAssembly()?");
            sb.I(4).Al(".GetReferencedAssemblies()");
            sb.I(4).Al(".Select(a => a.FullName);");
            sb.B();
            sb.I(3).Al("if (referencedAssemblies == null)");
            sb.I(4).Al("return null;");
            sb.B();
            sb.I(3).Al("return AppDomain.CurrentDomain.GetAssemblies()");
            sb.I(4).Al(".Where(a => referencedAssemblies.Contains(a.FullName))");
            sb.I(4).Al(".SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName || x.Name == typeName))");
            sb.I(4).Al(".FirstOrDefault();");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static Type? GetFirstMatchingTypeFromCurrentDomainAssembly(string typeName)");
            sb.I(2).Al("{");
            sb.I(3).Al("return AppDomain.CurrentDomain.GetAssemblies()");
            sb.I(4).Al(".SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName || x.Name == typeName))");
            sb.I(4).Al(".FirstOrDefault();");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("TypeProvider.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
