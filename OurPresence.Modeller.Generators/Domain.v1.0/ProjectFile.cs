﻿using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class ProjectFile : IGenerator
    {
        private readonly Module _module;

        public ProjectFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var projectName = _module.Namespace+".Common";
            var project = (IProject)new Project(projectName) { Path = System.IO.Path.Combine("src", projectName) };

            var files = new FileGroup();
            project.AddFileGroup(files);

            var sb = new StringBuilder();
            sb.Al("<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.B();
            sb.I(1).Al("<PropertyGroup>");
            sb.I(2).Al("<TargetFramework>net5.0</TargetFramework>");
            sb.I(2).Al($"<RootNamespace>{project.Name}</RootNamespace>");
            sb.I(2).Al("<LangVersion>Preview</LangVersion>");
            sb.I(2).Al($"<Nullable>enable</Nullable>");
            sb.I(1).Al("</PropertyGroup>");
            sb.B();
            sb.Al("</Project>");

            var projectFile = new File(project.Name + ".csproj", sb.ToString());
            files.AddFile(projectFile);

            return project;
        }
    }
}
