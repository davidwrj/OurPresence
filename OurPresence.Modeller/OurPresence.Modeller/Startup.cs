using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace OurPresence.Modeller
{
    internal class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ModellerApp _options;

        public Startup(
            IWebHostEnvironment environment,
            ModellerApp options
        )
        {
            _environment = environment;
            _options = options;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //
            // hostBuilder
            //     .ConfigureHostConfiguration(configHost =>
            //     {
            //         configHost.SetBasePath(Directory.GetCurrentDirectory());
            //         configHost.AddJsonFile(HostSettings, true);
            //     })
            //     .ConfigureAppConfiguration((hostContext, configApp) =>
            //     {
            //         configApp.SetBasePath(Directory.GetCurrentDirectory());
            //         configApp.AddJsonFile(AppSettings, true);
            //         configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
            //             true);
            //     })
            //     .ConfigureLogging((context, builder) =>
            //     {
            //         Log.Logger = new LoggerConfiguration()
            //             .WriteTo.File("codegen.log")
            //             .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}")
            //             .CreateLogger();
            //     })

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
        }

        public void Configure(IApplicationBuilder app)
        {

        }
    }
}
