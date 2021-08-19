﻿// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
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
            var projectName = $"{_module.Company}.{_module.Project}.Core";
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
            sb.I(2).Al("<Nullable>enable</Nullable>");
            sb.I(2).Al("<TreatWarningsAsErrors>true</TreatWarningsAsErrors>");
            sb.I(1).Al("</PropertyGroup>");
            sb.B();
            sb.I(1).Al("<ItemGroup>");
            sb.I(2).Al($"<PackageReference Include=\"FluentAssertions\" Version=\"{Settings.Packages.GetVersion("FluentAssertions", "6.0.0")}\" />");
            sb.I(2).Al($"<PackageReference Include=\"MediatR\" Version=\"{Settings.Packages.GetVersion("MediatR", "9.0.0")}\" />");
            sb.I(2).Al($"<PackageReference Include=\"Microsoft.AspNetCore.Mvc.Core\" Version=\"{Settings.Packages.GetVersion("Microsoft.AspNetCore.Mvc.Core", "2.2.5")}\" />");
            sb.I(2).Al($"<PackageReference Include=\"Microsoft.Extensions.DependencyInjection.Abstractions\" Version=\"{Settings.Packages.GetVersion("Microsoft.Extensions.DependencyInjection.Abstractions", "5.0.0")}\" />");
            sb.I(2).Al($"<PackageReference Include=\"RestSharp\" Version=\"{Settings.Packages.GetVersion("RestSharp", "106.12.0")}\" />");
            sb.I(1).Al("</ItemGroup>");
            sb.B();
            sb.Al("</Project>");

            var projectFile = new File(project.Name + ".csproj", sb.ToString());
            files.AddFile(projectFile);

            return project;
        }
    }
}
