using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainClass
{
    public class DomainCommand : IGenerator
    {
        private readonly Module _module;
        private readonly Request _request;

        public DomainCommand(ISettings settings, Module module, Request request)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();

            sb.al("using System;");
            sb.al("using System.Threading;");
            sb.al("using System.Threading.Tasks;");
            sb.al("using MediatR;");
            sb.b();
            sb.al($"namespace {_module.Namespace}.Commands");
            sb.al("{");
            sb.i(1).al($"public class {_request.Name} : INotification");
            sb.i(1).al("{");
            sb.i(2).a($"public {_request.Name}(");
            foreach (var field in _request.Fields)
            {
                sb.a($"{field.GetDataType()} {field.Name.Singular.LocalVariable}, ");
            }
            sb.TrimEnd(", ");
            sb.al(")");
            sb.i(2).al("{");
            foreach (var field in _request.Fields)
            {
                sb.i(3).al($"{field.Name.Value} = {field.Name.Singular.LocalVariable};");
            }
            sb.i(2).al("}");
            sb.b();
            foreach (var item in _request.Fields)
            {
                var property = (ISnippet)new Property.Generator(item, setScope: Property.PropertyScope.notAvalable).Create();
                sb.al(property.Content);
            }
            sb.i(1).al("}");
            sb.b();
            sb.i(1).al($"public class {_request.Name}Handler : INotificationHandler<{_request.Name}>");
            sb.i(1).al("{");
            sb.i(2).al($"public async Task Handle({_request.Name} command, CancellationToken cancellationToken)");
            sb.i(2).al("{");
            sb.i(3).al("// todo: Add code to complete the command");
            sb.i(2).al("}");
            sb.i(1).al("}");
            sb.al("}");

            return new File(_request.Name + ".cs", sb.ToString(), path: "Commands");
        }
    }
}