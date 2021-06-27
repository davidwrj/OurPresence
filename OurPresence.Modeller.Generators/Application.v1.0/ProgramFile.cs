using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace ApplicationProject
{
    internal class ProgramFile : IGenerator
    {
        private readonly Module _module;

        public ProgramFile(ISettings settings, Module module)
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
            sb.I(1).Al("public class Program");
            sb.I(1).Al("{");
            sb.I(2).Al("public static void Main(string[] args)");
            sb.I(2).Al("{");
            sb.I(3).Al("CreateHostBuilder(args).Build().Run();");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static IHostBuilder CreateHostBuilder(string[] args) =>");
            sb.I(3).Al("Host.CreateDefaultBuilder(args)");
            sb.I(4).Al(".ConfigureWebHostDefaults(webBuilder =>");
            sb.I(4).Al("{");
            sb.I(5).Al("webBuilder.UseStartup<Startup>();");
            sb.I(4).Al("});");
            sb.I(1).Al("}");
            sb.Al("}");
            
            return new File("Program.cs", sb.ToString());
        }
    }
}
