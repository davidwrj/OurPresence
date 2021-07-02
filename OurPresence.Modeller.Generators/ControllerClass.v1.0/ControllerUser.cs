using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace ControllerClass
{
    public class ControllerUser : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;

        public ControllerUser(ISettings settings, Module module, Model model)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public IOutput Create()
        {
            if(!Settings.SupportRegen)
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Controllers");
            sb.Al("{");
            sb.I(1).Al($"partial class {_model.Name}Controller : ApplicationController");
            sb.I(1).Al("{");
            sb.I(2).Al($"public {_model.Name}Controller(IMediator mediator)");
            sb.I(2).Al("{");
            sb.I(3).Al("_mediator = mediator;");
            sb.I(2).Al("}");


            sb.I(1).Al("}");
            sb.Al("}");

            var filename = _model.Name.ToString();
            filename += "Controller.cs";

            return new File(filename, sb.ToString(), canOverwrite: Settings.SupportRegen);
        }

        public ISettings Settings { get; }
    }
}
