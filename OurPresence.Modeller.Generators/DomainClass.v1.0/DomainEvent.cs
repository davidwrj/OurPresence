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

                    sb.al("using System;");
                    sb.al($"using {_module.Namespace}.Models;");
                    sb.al("");
                    sb.al($"namespace {_module.Namespace}.Events");
                    sb.al("{");
                    sb.i(1).al($"public class {behaviour.Event} : BaseDomainEvent<{_model.Name}, Guid>");
                    sb.i(1).al("{");
                    sb.i(2).al("/// <summary>");
                    sb.i(2).al("/// for deserialization");
                    sb.i(2).al("/// </summary>");
                    sb.i(2).al($"private {behaviour.Event}() {{ }}");
                    sb.b();
                    sb.i(2).al($"public {behaviour.Event}({_model.Name} {_model.Name.Singular.LocalVariable}) : base({_model.Name.Singular.LocalVariable})");
                    sb.i(2).al("{");
                    sb.i(3).al("// todo: ");
                    sb.i(2).al("}");
                    sb.i(1).al("}");
                    sb.al("}");

                    fileGroup.AddFile(new File(behaviour.Event + ".cs", sb.ToString(), path: "Events"));
                }
            }

            return fileGroup;
        }
    }
}