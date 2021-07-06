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
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al("global using System;");
            sb.Al("global using System.Collections.Generic;");
            sb.Al("global using System.Linq;");
            sb.Al("global using System.Threading.Tasks;");
            sb.Al("global using System.Net;");
            sb.Al("global using System.Text.Json;");
            sb.Al("global using CSharpFunctionalExtensions;");
            sb.Al("global using FluentValidation.AspNetCore;");
            sb.Al("global using MediatR;");
            sb.Al("global using Microsoft.AspNetCore.Authorization;");
            sb.Al("global using Microsoft.AspNetCore.Builder;");
            sb.Al("global using Microsoft.AspNetCore.Hosting;");
            sb.Al("global using Microsoft.AspNetCore.Http;");
            sb.Al("global using Microsoft.AspNetCore.Mvc;");
            sb.Al("global using Microsoft.AspNetCore.Mvc.ModelBinding;");
            sb.Al("global using Microsoft.Extensions.Configuration;");
            sb.Al("global using Microsoft.Extensions.DependencyInjection;");
            sb.Al("global using Microsoft.Extensions.Hosting;");
            sb.Al("global using Microsoft.OpenApi.Models;");
            sb.B();
            sb.Al($"global using {_module.Namespace}.Common;");

            return new File("usings.generated.cs", sb.ToString(), canOverwrite: true);
        }
    }
}
