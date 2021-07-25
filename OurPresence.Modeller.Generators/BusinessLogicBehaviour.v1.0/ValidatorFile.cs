// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace BusinessLogicBehaviour
{
    public class ValidatorFile : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;
        private readonly Behaviour _behaviour;

        public ValidatorFile(ISettings settings, Module module, Model model, Behaviour behaviour)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _behaviour = behaviour ?? throw new ArgumentNullException(nameof(behaviour)); ;
        }

        public IOutput Create()
        {
            if(_behaviour.Request is null)
                return null;

            var sb = new StringBuilder();
            sb.Al(((ISnippet)new Header.Generator(Settings, new GeneratorDetails()).Create()).Content);
            sb.Al($"using {_module.Namespace}.BusinessLogic.{_model.Name}.{_behaviour.Name};");
            sb.B();
            sb.Al($"namespace {_module.Namespace}.BusinessLogic.{_model.Name}.Validators");
            sb.Al("{");
            var request = _behaviour.Request is null ?
                new Name($"{_model.Name}{_behaviour.Name}Request") :
                _behaviour.Request.Name;

            sb.I(1).Al($"public class {request}Validator : AbstractValidator<{request}>");
            sb.I(1).Al("{");
            sb.I(2).Al($"public {request}Validator()");
            sb.I(2).Al("{");
            foreach (var field in _behaviour.Request.Fields)
            {
                if(field.Nullable && !field.Scale.HasValue && !field.MaxLength.HasValue && !field.MinLength.HasValue)
                {
                    continue;
                }

                sb.I(3).A($"RuleFor(p => p.{field.Name})");
                if (!field.Nullable)
                {
                    sb.A($".NotNull()");
                }
                switch (field.DataType)                    
                {
                    case DataTypes.Decimal:
                        if (field.Precision.HasValue && field.Scale.HasValue)
                        {
                            sb.A($".ScalePrecision({field.Scale}, {field.Precision})");
                        }
                        break;
                    case DataTypes.String:
                        if(field.MaxLength.HasValue)
                        {
                            sb.A($".MaximumLength({ field.MaxLength})");
                        }
                        if(field.MinLength.HasValue)
                        {
                            sb.A($".MinimumLength({ field.MaxLength})");
                        }
                        break;
                    default:
                        break;
                }
                sb.Al(";");
            }
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File(request + "Validator.cs", sb.ToString(), path: System.IO.Path.Combine(_model.Name.ToString(), "Validators"));
        }

        public ISettings Settings { get; }
    }
}
