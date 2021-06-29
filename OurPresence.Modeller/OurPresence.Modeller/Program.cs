// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller
{
    [Command(Name = "codegen", Description = "CodeGen tool is used to generate code via DLL plug-ins.")]
    internal class Program
    {
        private const string Appsettings = "appsettings.json";
        private const string Hostsettings = "hostsettings.json";

        public static async Task<int> Main(string[] args)
        {
            var hostBuilder = new HostBuilder();
            try
            {
                hostBuilder
                    .ConfigureHostConfiguration(configHost =>
                    {
                        configHost.SetBasePath(Directory.GetCurrentDirectory());
                        configHost.AddJsonFile(Hostsettings, optional: true);
                    })
                    .ConfigureAppConfiguration((hostContext, configApp) =>
                    {
                        configApp.SetBasePath(Directory.GetCurrentDirectory());
                        configApp.AddJsonFile(Appsettings, optional: true);
                        configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    })
                    .ConfigureLogging((context, builder) =>
                    {
                        Log.Logger = new LoggerConfiguration()
                            .WriteTo.File("codegen.log")
                            .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}")
                            .CreateLogger();
                    })
                    .ConfigureServices((context, services) =>
                    {
                        services.AddLogging(configure => configure.AddSerilog());

                        services.AddSingleton<ISettings, Generator.Settings>();
                        services.AddSingleton<IGeneratorConfiguration, GeneratorConfiguration>();
                        services.AddSingleton<IContext, Context>();

                        services.AddScoped<ILoader<ISettings>, JsonSettingsLoader>();
                        services.AddScoped<ILoader<IEnumerable<INamedElement>>, JsonModuleLoader>();
                        services.AddScoped<ILoader<IEnumerable<IGeneratorItem>>, GeneratorLoader>();
                        services.AddScoped<ICodeGenerator, CodeGenerator>();
                        services.AddScoped<IPresenter, Presenter>();
                        services.AddScoped<IBuilder, Builder>();
                        services.AddScoped<IUpdater, Updater>();
                        services.AddScoped<IPackageService, PackageService>();
                        services.AddScoped<ILoader<IEnumerable<IPackage>>, PackageFileLoader>();

                        services.AddTransient<IFileWriter, FileWriter>();

                        services.AddScoped<IOutputStrategy, OutputStrategy>();
                        services.AddScoped<IFileCreator, CreateFile>();
                        services.AddScoped<IFileCreator, CreateSnippet>();
                        services.AddScoped<IFileCreator, CreateProject>();
                        services.AddScoped<IFileCreator, CreateSolution>();
                        services.AddScoped<IFileCreator, FileCopier>();
                        services.AddScoped<IFileCreator, FolderCopier>();
                        services.AddScoped<IFileCreator, CreateFileGroup>();
                    });

                return await hostBuilder
                    .RunCommandLineApplicationAsync<ModellerApp>(args)
                    .ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "CodeGen program.cs caught an issue");
                return 1;
            }
        }
    }
}
