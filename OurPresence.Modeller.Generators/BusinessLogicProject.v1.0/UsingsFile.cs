﻿// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class UsingsFile : IGenerator
    {
        private readonly Module _module;

        public UsingsFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al("global using System;");
            sb.Al("global using System.Collections.Generic;");
            sb.Al("global using System.Linq;");
            sb.Al("global using System.Text;");
            sb.Al("global using System.Threading;");
            sb.Al("global using System.Threading.Tasks;");
            sb.Al("global using CSharpFunctionalExtensions;");
            sb.Al("global using FluentValidation;");
            sb.Al($"global using {_module.Company}.{_module.Project}.Core.Queries;");
            sb.Al($"global using {_module.Company}.{_module.Project}.Core.Commands;");
            sb.B();
            sb.Al($"global using {_module.Namespace}.Common.Enums;");
            return new File("usings.cs", sb.ToString(), canOverwrite: true);
        }
    }
}
