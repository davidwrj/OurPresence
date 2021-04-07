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

            sb.Al("using System;");
            sb.Al("using System.Threading;");
            sb.Al("using System.Threading.Tasks;");
            sb.Al("using MediatR;");
            sb.B();
            sb.Al($"namespace {_module.Namespace}.Commands");
            sb.Al("{");
            sb.I(1).Al($"public class {_request.Name} : INotification");
            sb.I(1).Al("{");
            sb.I(2).A($"public {_request.Name}(");
            foreach (var field in _request.Fields)
            {
                sb.A($"{field.GetDataType()} {field.Name.Singular.LocalVariable}, ");
            }
            sb.TrimEnd(", ");
            sb.Al(")");
            sb.I(2).Al("{");
            foreach (var field in _request.Fields)
            {
                sb.I(3).Al($"{field.Name.Value} = {field.Name.Singular.LocalVariable};");
            }
            sb.I(2).Al("}");
            sb.B();
            foreach (var item in _request.Fields)
            {
                var property = (ISnippet)new Property.Generator(item, setScope: Property.PropertyScope.NotAvailable).Create();
                sb.Al(property.Content);
            }
            sb.I(1).Al("}");
            sb.B();
            sb.I(1).Al($"public class {_request.Name}Handler : INotificationHandler<{_request.Name}>");
            sb.I(1).Al("{");
            sb.I(2).Al($"public async Task Handle({_request.Name} command, CancellationToken cancellationToken)");
            sb.I(2).Al("{");
            sb.I(3).Al("// todo: Add code to complete the command");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File(_request.Name + ".cs", sb.ToString(), path: "Commands");
        }
    }
}