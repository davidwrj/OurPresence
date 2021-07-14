// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class ExtensionsFileGroup : IGenerator
    {
        private readonly Module _module;

        public ExtensionsFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Extensions");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Core.Extensions");
            sb.Al("{");
            sb.I(1).Al("public static class ListExtensions");
            sb.I(1).Al("{");
            sb.I(2).Al("public static IList<T> Replace<T>(this IList<T> list, T existingElement, T replacement)");
            sb.I(2).Al("{");
            sb.I(3).Al("var indexOfExistingItem = list.IndexOf(existingElement);");
            sb.I(3).Al("if (indexOfExistingItem == -1)");
            sb.I(4).Al("throw new ArgumentOutOfRangeException(nameof(existingElement), \"Element was not found\");");
            sb.B();
            sb.I(3).Al("list[indexOfExistingItem] = replacement;");
            sb.B();
            sb.I(3).Al("return list;");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            group.AddFile(new File("ListExtensions.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
