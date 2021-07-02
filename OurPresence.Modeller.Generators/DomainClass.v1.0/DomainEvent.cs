using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainClass
{
    public class DomainEvent : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;

        public DomainEvent(ISettings settings, Module module, Model model)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var fileGroup = new FileGroup();
            foreach (var behaviour in _model.Behaviours)
            {
                if(!string.IsNullOrWhiteSpace(behaviour.Event))
                {
                    var sb = new StringBuilder();

                    sb.Al($"namespace {_module.Namespace}.Common.Domain.Events");
                    sb.Al("{");
                    sb.I(1).Al($"public class {behaviour.Event} : BaseDomainEvent<{_model.Name}, Guid>");
                    sb.I(1).Al("{");
                    sb.I(2).Al("/// <summary>");
                    sb.I(2).Al("/// for deserialization");
                    sb.I(2).Al("/// </summary>");
                    sb.I(2).Al($"private {behaviour.Event}() {{ }}");
                    sb.B();
                    sb.I(2).Al($"public {behaviour.Event}({_model.Name} {_model.Name.Singular.LocalVariable}) : base({_model.Name.Singular.LocalVariable})");
                    sb.I(2).Al("{");
                    sb.I(3).Al("// todo: ");
                    sb.I(2).Al("}");
                    sb.I(1).Al("}");
                    sb.Al("}");

                    fileGroup.AddFile(new File(behaviour.Event + ".cs", sb.ToString(), path: "Events"));
                }
            }

            return fileGroup;
        }
    }
}
