﻿// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace BusinessLogicProject
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
            var projectName = _module.Namespace + ".BusinessLogic";
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
            sb.I(1).Al("<ItemGroup>");
            sb.I(2).Al($"<PackageReference Include=\"CSharpFunctionalExtensions\" Version=\"{Settings.Packages.GetVersion("CSharpFunctionalExtensions", "2.18.0")}\" />");
            sb.I(2).Al($"<PackageReference Include=\"FluentValidation\" Version=\"{Settings.Packages.GetVersion("FluentValidation", "10.3.0")}\" />");
            sb.I(1).Al("</ItemGroup>");
            sb.B();
            sb.I(1).Al("<ItemGroup>");
            if (string.IsNullOrEmpty(_module.Feature?.Value))
            {
                sb.I(2).Al($"<ProjectReference Include=\"..\\{_module.Company}.{_module.Project}.Common\\{_module.Company}.{_module.Project}.Common.csproj\" />");
            }
            else
            {
                sb.I(2).Al($"<ProjectReference Include=\"..\\{_module.Company}.{_module.Project}.{_module.Feature}.Common\\{_module.Company}.{_module.Project}.{_module.Feature}.Common.csproj\" />");
            }
            sb.I(1).Al("</ItemGroup>");
            sb.B();
            sb.Al("</Project>");

            files.AddFile(new File(project.Name + ".csproj", sb.ToString()));

            return project;
        }
    }
}
