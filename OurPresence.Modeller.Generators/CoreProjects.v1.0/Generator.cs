// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using DomainProject;
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
            project.FileGroups.First().AddFile((IFile)new UsingsFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new ConfigFile(Settings, _module).Create());

            project.AddFileGroup((IFileGroup)new AggregateFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new BaseAsyncEndpointGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new CommandFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new EventsFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new ExceptionsFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new ExtensionsFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new IdsFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new ProjectionsFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new QueryFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new ReflectionFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new RepositoriesFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new RequestsFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new ResponsesFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new SubscriptionsFileGroup(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new ThreadingFileGroup(Settings, _module).Create());

            return project;
        }
    }
}
