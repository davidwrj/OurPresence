using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DotNetConfig;
using McMaster.Extensions.CommandLineUtils;

namespace OurPresence.Modeller
{
    [Command(Name = "codegen", Description = "CodeGen tool is used to generate code via DLL plug-ins.")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    public class Program
    {
        private const string AppSettings = "appsettings.json";
        private const string HostSettings = "hostsettings.json";

        internal static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? "0.0";

        public static int Main(string[] args)
        {
            DebugHelper.HandleDebugSwitch(ref args);
            return new Program().Run(args);
        }

        internal int Run(params string[] args)
        {
            try
            {
                using var app = new CommandLineApplication<ModellerApp>();
                app.Conventions.UseDefaultConventions();
                app.OnExecuteAsync(async ct =>
                {
                    // NOTE: this isn't very elegant, but moving this logic
                    // down to the CommandLineUtils level proved quite challenging
                    // and potentially not that useful.
                    var config = Config.Build(app.Model.Settings).GetSection("serve");

                    ReadConfig(config, app.Model);
                    if (app.Model.SaveOptions)
                    {
                        WriteConfig(config, app.Model);
                    }
                    return await OnRunAsync(app.Model, ct);
                });
                app.OnValidationError(r => Error(r.ErrorMessage));
                return app.Execute(args);
            }
            catch (Exception ex)
            {
                Error("CodeGen program.cs caught an issue" + ex);
                return 2;
            }
        }

        private readonly List<string> _errors = new();
        public IEnumerable<string> Errors { get { return _errors; } }
        private void Error(string? message)
        {
            if (message is null) return;
            _errors.Add(message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ResetColor();
        }

        protected virtual Task<int> OnRunAsync(ModellerApp options, CancellationToken ct)
        {
            // var server = new SimpleServer(options, PhysicalConsole.Singleton, Directory.GetCurrentDirectory());
            // return server.RunAsync(ct);

            return Task.FromResult(0);
        }

        private static void ReadConfig(ConfigSection config, ModellerApp model)
        {
            // model.Port ??= (int?)config.GetNumber("port");
            // model.Directory ??= config.GetString("directory");
        }

        private static void WriteConfig(ConfigSection config, ModellerApp model)
        {
            // if (model.Port != null)
            // {
            //     config.SetNumber("port", model.Port.GetValueOrDefault());
            // }
            // if (model.Directory != null)
            // {
            //     config.SetString("directory", model.Directory);
            // }
        }
    }
}
