// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Interfaces;
using System;
using System.Linq;

namespace CoreProjects
{
    public class Generator : IGenerator
    {
        private readonly Module _module;

        public Generator(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var project = (IProject)new ProjectFile(Settings, _module).Create();
            //project.FileGroups.First().AddFile((IFile)new AppSettingsFile(Settings, _module).Create());

            project.AddFileGroup((IFileGroup)new AggregateFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new CommandFileGroup(Settings, _module).Create());

            return project;
        }
    }
}
