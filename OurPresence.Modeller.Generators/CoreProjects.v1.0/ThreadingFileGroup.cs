// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class ThreadingFileGroup : IGenerator
    {
        private readonly Module _module;

        public ThreadingFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Threading");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Core.Threading");
            sb.Al("{");
            sb.I(1).Al("public static class NoSynchronizationContextScope");
            sb.I(1).Al("{");
            sb.I(2).Al("public static Disposable Enter()");

            sb.I(2).Al("{");
            sb.I(3).Al("var context = SynchronizationContext.Current;");
            sb.I(3).Al("SynchronizationContext.SetSynchronizationContext(null);");
            sb.I(3).Al("return new Disposable(context);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public struct Disposable: IDisposable");
            sb.I(2).Al("{");
            sb.I(3).Al("private readonly SynchronizationContext? _synchronizationContext;");
            sb.B();
            sb.I(3).Al("public Disposable(SynchronizationContext? synchronizationContext)");
            sb.I(3).Al("{");
            sb.I(4).Al("_synchronizationContext = synchronizationContext;");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("public void Dispose() =>");
            sb.I(4).Al("SynchronizationContext.SetSynchronizationContext(_synchronizationContext);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("NoSynchronizationContextScope.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
