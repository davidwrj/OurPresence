// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
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
            sb.I(1).A(Settings.SupportRegen ? $"partial record" : $"public class");
            sb.Al($" {_model.Name}{_behaviour.Name}Request : IRequest<{_model.Name}, ");

            sb.Al("}");

            var filename = _model.Name.ToString();
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
