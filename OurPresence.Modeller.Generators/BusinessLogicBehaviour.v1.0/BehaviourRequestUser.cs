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
    public class BehaviourRequestUser : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;
        private readonly Behaviour _behaviour;

        public BehaviourRequestUser(ISettings settings, Module module, Model model, Behaviour behaviour)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _behaviour = behaviour ?? throw new ArgumentNullException(nameof(behaviour)); ;
        }

        public IOutput Create()
        {
            if (!Settings.SupportRegen)
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.Al(((ISnippet)new Header.Generator(Settings, new GeneratorDetails()).Create()).Content);
            sb.Al($"namespace {_module.Namespace}.BusinessLogic.{_model.Name}.{_behaviour.Name}");
            sb.Al("{");
            var request = _behaviour.Request is null ?
                new Name($"{_model.Name}{_behaviour.Name}Request") :
                _behaviour.Request.Name;

            sb.I(1).A($"public class {_model.Name}{_behaviour.Name}Handler : IRequestHandler<{request}, Result");
            sb.A(_behaviour.Response.IsCollection ? "<IEnumerable" : "");
            sb.A($"<{_behaviour.Response.Name}>");
            sb.A(_behaviour.Response.IsCollection ? ">>" : ">");
            sb.I(1).Al("{");
            sb.I(2).A($"public async Task<Result");
            sb.A(_behaviour.Response.IsCollection ? "<IEnumerable" : "");
            sb.A($"<{_behaviour.Response.Name}>");
            sb.A(_behaviour.Response.IsCollection ? ">>" : ">");
            sb.Al($" Handle({request} request, CancellationToken cancellationToken)");
            sb.I(2).Al("{");
            sb.I(3).Al("return null;");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            var filename = $"{_model.Name}{_behaviour.Name}Handler.cs";
            return new File(filename, sb.ToString(), path: System.IO.Path.Combine(_model.Name.ToString(), _behaviour.Name.ToString()), canOverwrite: Settings.SupportRegen);
        }

        public ISettings Settings { get; }
    }
}
