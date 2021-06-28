// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;

namespace EntityFrameworkClass
{
    public class ModelGenerated : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;

        public ModelGenerated(ISettings settings, Module module, Model model)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(module));
        }

        public IOutput Create()
        {
            var bk = _model.HasBusinessKey();
            var isEntity = _model.IsEntity();

            var sb = new StringBuilder();
            if(Settings.SupportRegen)
            {
                sb.Al(((ISnippet)new OverwriteHeader.Generator(Settings, new GeneratorDetails()).Create()).Content);
            }
            else
            {
                sb.Al(((ISnippet)new Header.Generator(Settings, new GeneratorDetails()).Create()).Content);
            }
            sb.Al($"namespace {_module.Namespace}.Data.Models");
            sb.Al("{");
            sb.I(1).A(Settings.SupportRegen ? $"partial class {_model.Name}" : $"public class {_model.Name}");
            var entity = string.Empty;
            if(isEntity)
            {
                entity += $" : AuditableBase";
            }
            sb.Al(entity);
            sb.I(1).Al("{");

            foreach(var item in _model.Key.Fields)
            {
                if(isEntity && item.Name.ToString() == "Id")
                    continue;
                var property = (ISnippet)new Property.Generator(item, setScope: Property.PropertyScope.Private).Create();
                sb.Al(property.Content);
            }
            foreach(var item in _model.Fields)
            {
                var property = (ISnippet)new Property.Generator(item, setScope: Property.PropertyScope.Private).Create();
                sb.Al(property.Content);
            }

            sb.I(1).Al("}");
            sb.Al("}");

            var filename = _model.Name.ToString();
            if(Settings.SupportRegen)
            {
                filename += ".generated";
            }
            filename += ".cs";

            return new File(filename, sb.ToString(), canOverwrite: Settings.SupportRegen);
        }

        public ISettings Settings { get; }
    }
}
