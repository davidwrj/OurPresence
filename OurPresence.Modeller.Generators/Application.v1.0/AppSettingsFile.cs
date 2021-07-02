using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace ApplicationProject
{
    internal class AppSettingsFile : IGenerator
    {
        private readonly Module _module;

        public AppSettingsFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al("{");
            sb.Al("  \"Logging\": {");
            sb.Al("    \"LogLevel\": {");
            sb.Al("      \"Default\": \"Information\",");
            sb.Al("      \"Microsoft\": \"Warning\",");
            sb.Al("      \"Microsoft.Hosting.Lifetime\": \"Information\"");
            sb.Al("    }");
            sb.Al("  },");
            sb.Al("  \"AllowedHosts\": \"*\"");
            sb.Al("}");
            
            return new File("appsettings.json", sb.ToString());
        }
    }
}
