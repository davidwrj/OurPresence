// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace EntityFrameworkProject
{
    internal class DbContextFile : IGenerator
    {
        private readonly Module _module;

        public DbContextFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Common.Enums");
            sb.Al("{");
            sb.I(1).Al($"public class {_module.Project}DBContext: DbContext");
            sb.I(1).Al("{");
            sb.I(2).Al($"public {_module.Project}DBContext(DbContextOptions<{_module.Project}DBContext> options)");
            sb.I(3).Al(": base(options)");
            sb.I(2).Al("{");
            sb.I(1).Al("");
            sb.I(2).Al("}");
            sb.I(1).Al("");
            sb.I(2).Al("protected override void OnModelCreating(ModelBuilder modelBuilder)");
            sb.I(2).Al("{");
            sb.I(3).Al("modelBuilder.SetupProductsModel();");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.I(1).Al("");
            sb.I(1).Al($"public class {_module.Project}DBContextFactory: IDesignTimeDbContextFactory<{_module.Project}DBContext>");
            sb.I(1).Al("{");
            sb.I(2).Al($"public {_module.Project}DBContext CreateDbContext(params string[] args)");
            sb.I(2).Al("{");
            sb.I(3).Al($"var optionsBuilder = new DbContextOptionsBuilder<{_module.Project}DBContext>();");
            sb.I(1).Al("");
            sb.I(3).Al("if (optionsBuilder.IsConfigured)");
            sb.I(4).Al($"return new {_module.Project}DBContext(optionsBuilder.Options);");
            sb.I(1).Al("");
            sb.I(3).Al("//Called by parameterless ctor Usually Migrations");
            sb.I(3).Al("var environmentName = Environment.GetEnvironmentVariable(\"EnvironmentName\") ?? \"Development\";");
            sb.I(1).Al("");
            sb.I(3).Al("var connectionString =");
            sb.I(4).Al("new ConfigurationBuilder()");
            sb.I(5).Al(".SetBasePath(AppContext.BaseDirectory)");
            sb.I(5).Al(".AddJsonFile(\"appsettings.json\")");
            sb.I(5).Al(".AddJsonFile($\"appsettings.{environmentName}.json\", optional: true, reloadOnChange: false)");
            sb.I(5).Al(".AddEnvironmentVariables()");
            sb.I(5).Al(".Build()");
            sb.I(5).Al($".GetConnectionString(\"{_module.Project}DB\");");
            sb.I(1).Al("");
            sb.I(3).Al("optionsBuilder.UseNpgsql(connectionString);");
            sb.I(1).Al("");
            sb.I(3).Al($"return new {_module.Project}DBContext(optionsBuilder.Options);");
            sb.I(2).Al("}");
            sb.I(1).Al("");
            sb.I(2).Al($"public static {_module.Project}DBContext Create()");
            sb.I(3).Al($"=> new {_module.Project}DBContextFactory().CreateDbContext();");
            sb.I(1).Al("}");
            sb.Al("}");

            var filename = $"{_module.Project}DbContext";
            if (Settings.SupportRegen)
            {
                filename += ".generated";
            }
            filename += ".cs";

            return new File(filename, sb.ToString(), canOverwrite: true, path:"Storage");
        }
    }
}
