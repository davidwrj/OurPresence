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
    public class DomainUser : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;

        public DomainUser(ISettings settings, Module module, Model model)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            if (!Settings.SupportRegen)
            {
                return null;
            }

            var relationships = _module.FindRelationshipsByModel(_model);

            var sb = new StringBuilder();
            sb.Al(((ISnippet)new Header.Generator(Settings, new GeneratorDetails()).Create()).Content);
            sb.Al($"namespace {_module.Namespace}.Common.Domain");
            sb.Al("{");
            sb.I(1).Al($"public partial class {_model.Name}");
            sb.I(1).Al("{");
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

            var p = "public partial ";
            foreach (var item in _model.Behaviours)
            {
                sb.I(2).A($"{p}void {item.Name}(");
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
                sb.B();
            }

            //sb.I(2).Al($"protected override void Apply(IDomainEvent<Guid> @event)");
            //sb.I(2).Al("{");
            //sb.I(3).Al($"// todo: Apply events");
            //sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File(_model.Name + ".cs", sb.ToString());
        }
    }
}
