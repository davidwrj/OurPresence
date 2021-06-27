using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace ApplicationProject
{
    internal class StartupFile : IGenerator
    {
        private readonly Module _module;

        public StartupFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Application");
            sb.Al("{");
            sb.I(1).Al("public class Startup");
            sb.I(1).Al("{");
            sb.I(2).Al("public Startup(IConfiguration configuration)");
            sb.I(2).Al("{");
            sb.I(3).Al("Configuration = configuration;");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public IConfiguration Configuration { get; }");

            sb.I(1).Al("}");
            sb.Al("}");
            
            return new File("Startup.cs", sb.ToString());
        }
    }
}
