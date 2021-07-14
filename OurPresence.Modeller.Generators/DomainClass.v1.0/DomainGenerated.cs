// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Linq;
using System.Text;

namespace DomainClass
{
    public class DomainGenerated : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;

        public DomainGenerated(ISettings settings, Module module, Model model)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public IOutput Create()
        {
            var bk = _model.HasBusinessKey();
            var isEntity = _model.IsEntity();

            var sb = new StringBuilder();
            if (Settings.SupportRegen)
            {
                sb.Al(((ISnippet)new OverwriteHeader.Generator(Settings, new GeneratorDetails()).Create()).Content);
            }
            else
            {
                sb.Al(((ISnippet)new Header.Generator(Settings, new GeneratorDetails()).Create()).Content);
            }
            sb.Al($"namespace {_module.Namespace}.Common.Domain");
            sb.Al("{");
            sb.I(1).A(Settings.SupportRegen ? $"partial class {_model.Name}" : $"public class {_model.Name}");
            var entity = string.Empty;
            if (isEntity)
            {
                entity += $" : BaseAggregateRoot<{_model.Name}, ";
                if(_model.Key.Fields.Count==1)
                {
                    entity += $"{_model.Key.Fields.First().GetDataType()}>";
                }
                else
                {
                    entity += "{ ";
                    foreach (var item in _model.Key.Fields)
                    {
                        entity += item.GetDataType() + ",";
                    }
                    entity.TrimEnd(',');
                    entity += " }>;";
                }
            }
            sb.Al(entity);
            sb.I(1).Al("{");

            if (!Settings.SupportRegen)
            {
                sb.I(2).A($"public {_model.Name}(");
                foreach (var field in _model.Fields)
                {
                    sb.A($"{field.GetDataType()} {field.Name.Singular.LocalVariable}, ");
                }
                sb.TrimEnd(", ");
                sb.Al(")");
                sb.I(2).Al("{");
                foreach (var field in _model.Fields.Where(f => !f.Nullable && f.DataType == DataTypes.String))
                {
                    sb.I(3).Al($"if(string.IsNullOrWhiteSpace({field.Name.Singular.LocalVariable}))");
                    sb.I(4).Al($"throw new ArgumentException(\"Must include a value for {field.Name.Singular.Display}\");");
                }

                foreach (var field in _model.Fields)
                {
                    sb.I(3).Al($"{field.Name.Value} = {field.Name.Singular.LocalVariable};");
                }
                sb.I(2).Al("}");
                sb.B();
            }

            foreach (var item in _model.Key.Fields)
            {
                if (isEntity && item.Name.ToString() == "Id")
                    continue;
                var property = (ISnippet)new Property.Generator(item, setScope: Property.PropertyScope.Private).Create();
                sb.Al(property.Content);
            }
            foreach (var item in _model.Fields)
            {
                var property = (ISnippet)new Property.Generator(item, setScope: Property.PropertyScope.Private).Create();
                sb.Al(property.Content);
            }

            foreach (var item in _model.Behaviours)
            {
                if (Settings.SupportRegen)
                {
                    sb.I(2).A($"public partial void {item.Name}(");
                    if(item.Request != null)
                    {
                        foreach(var field in item.Request.Fields)
                        {
                            sb.A($"{field.GetDataType()} {field.Name.Singular.LocalVariable}, ");
                        }
                        sb.TrimEnd(", ");
                    }
                    sb.Al(");");
                }
                else
                {
                    sb.I(2).A($"public void {item.Name}(");
                    if(item.Request != null)
                    {
                        foreach(var field in item.Request.Fields)
                        {
                            sb.A($"{field.GetDataType()} {field.Name.Singular.LocalVariable}, ");
                        }
                        sb.TrimEnd(", ");
                    }
                    sb.Al(")");
                    sb.I(2).Al("{");
                    sb.I(3).Al($"// todo: Add {item.Name.Singular.Display} behaviour here");
                    sb.I(2).Al("}");
                }
                sb.B();
            }

            if (!Settings.SupportRegen)
            {
                sb.I(2).Al($"protected override void Apply(IDomainEvent<Guid> @event)");
                sb.I(2).Al("{");
                sb.I(3).Al($"// todo: Apply events");
                sb.I(2).Al("}");
            }

            sb.TrimEnd(Environment.NewLine);
            sb.B();

            sb.I(1).Al("}");
            sb.Al("}");

            var filename = _model.Name.ToString();
            if (Settings.SupportRegen)
            {
                filename += ".generated";
            }
            filename += ".cs";

            return new File(filename, sb.ToString(), canOverwrite: Settings.SupportRegen);
        }

        public ISettings Settings { get; }
    }
}
