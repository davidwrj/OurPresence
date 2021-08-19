// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace BusinessLogicBehaviour
{
    public class BehaviourRequestGenerator : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;
        private readonly Behaviour _behaviour;

        public BehaviourRequestGenerator(ISettings settings, Module module, Model model, Behaviour behaviour)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _behaviour = behaviour ?? throw new ArgumentNullException(nameof(behaviour)); ;
        }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            if (Settings.SupportRegen)
            {
                sb.Al(((ISnippet)new OverwriteHeader.Generator(Settings, new GeneratorDetails()).Create()).Content);
            }
            else
            {
                sb.Al(((ISnippet)new Header.Generator(Settings, new GeneratorDetails()).Create()).Content);
            }
            sb.Al($"namespace {_module.Namespace}.BusinessLogic.{_model.Name}.{_behaviour.Name}");
            sb.Al("{");

            

            // Request
            var request = _behaviour.Request is null ?
            new Name($"{_model.Name}{_behaviour.Name}Request") :
            _behaviour.Request.Name;

            sb.I(1).A($"public partial record {request}(");
            if (_behaviour.Request is not null)
            {
                foreach (var field in _behaviour.Request.Fields)
                {
                    sb.A($"{field.GetDataType(true)} {field.Name}, ");
                }
                sb.TrimEnd(", ");
            }
            sb.Al(")");
            sb.I(2).A(": IQuery");
            if (_behaviour.Response is not null)
            {
                sb.A("<Result");
                sb.A(_behaviour.Response.IsCollection ? "<IEnumerable" : "");
                sb.A($"<{_behaviour.Response.Name}>");
                sb.A(_behaviour.Response.IsCollection ? ">>" : ">");
            }
            sb.B();
            sb.I(1).Al("{ }");
            sb.B();

            if (!Settings.SupportRegen)
            {
                // Handler
                sb.I(1).Al($"public class {_behaviour.Name} : IQueryHandler<{request}, {_behaviour.Response.Name}>");
                sb.I(1).Al("{");
                sb.I(2).Al($"public async Task<int> Handle({request} request, CancellationToken cancellationToken)");
                sb.I(2).Al("{");
                sb.I(2).Al("}");
                sb.I(1).Al("}");
            }

            // Response
            if (_behaviour.Response is not null)
            {
                sb.I(1).A($"public partial record {_behaviour.Response.Name}(");
                foreach (var field in _behaviour.Response.Fields)
                {
                    sb.A($"{field.GetDataType()} {field.Name}, ");
                }
                sb.TrimEnd(", ");
                sb.Al(")");
                sb.I(1).Al("{ }");
            }
            sb.Al("}");

            var filename = $"{_model.Name}{_behaviour.Name}";
            if (Settings.SupportRegen)
            {
                filename += ".generated";
            }
            filename += ".cs";

            return new File(filename, sb.ToString(), path: System.IO.Path.Combine(_model.Name.ToString(), _behaviour.Name.ToString()), canOverwrite: Settings.SupportRegen);
        }

        public ISettings Settings { get; }
    }
}
