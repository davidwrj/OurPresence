// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Linq;

namespace DomainProject
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
            project.FileGroups.First().AddFile((IFile)new EntityFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new EnumValueObjectFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new SimpleValueObjectFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new ValueObjectFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new ErrorFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new NestingFile(Settings).Create());
            project.FileGroups.First().AddFile((IFile)new UsingsFile(Settings, _module).Create());
            project.FileGroups.First().AddFile((IFile)new EmailSampleFile(Settings, _module).Create());
            project.AddFileGroup((IFileGroup)new DomainClass.Generator(Settings, _module, null).Create());

            var enums = new FileGroup("Enums");
            _module.Enumerations.ForEach(e => enums.AddFile((IFile)new EnumClass.Generator(Settings, _module, e).Create()));
            if (enums.Files.Any())
            {
                project.AddFileGroup(enums);
            }

            return project;
        }
    }
}
