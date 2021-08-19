// Copyright (c)  Allan Nielsen.
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
            _module = module;
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al("global using System;");
            sb.Al("global using System.Collections.Generic;");
            sb.Al("global using System.Linq;");
            sb.Al("global using System.Text;");
            sb.Al("global using System.Threading.Tasks;");
            sb.Al("global using Microsoft.AspNetCore.Routing;");
            sb.Al("global using Microsoft.EntityFrameworkCore;");
            sb.Al("global using Microsoft.EntityFrameworkCore.Design;");
            sb.Al("global using Microsoft.EntityFrameworkCore.Metadata.Builders;");
            sb.Al("global using Microsoft.Extensions.Configuration;");
            sb.Al("global using Microsoft.Extensions.DependencyInjection;");
            sb.B();
            sb.Al($"global using {_module.Namespace}.Common.Enums;");
            sb.Al($"global using {_module.Namespace}.Data.Models;");
            return new File("usings.cs", sb.ToString());
        }
    }
}
