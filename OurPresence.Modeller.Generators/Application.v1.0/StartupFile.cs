using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Linq;
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
            var m = _module.Models.Where(m => m.Behaviours.Any()).Select(m => new { Model = m, Behaviour = m.Behaviours.First() }).FirstOrDefault();

            sb.Al($"using {_module.Namespace}.BusinessLogic.Vehicle.Search;");
            sb.Al($"using {_module.Namespace}.BusinessLogic.Vehicle;");
            sb.Al($"using {_module.Namespace}.Common.EntityTypes;");
            sb.Al($"using {_module.Namespace}.DataAccess.Rcms;");
            sb.Al($"using {_module.Namespace}.DataAccess.Nevdis;");
            sb.B();
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
            sb.B();
            sb.I(2).Al("public void ConfigureServices(IServiceCollection services)");
            sb.I(2).Al("{");
            if(m != null)
            {
                sb.I(3).Al($"services.AddMediatR(typeof({m.Model.Name}{m.Behaviour.Name}Request).Assembly);");
                sb.B();
            }
            sb.I(3).Al("services.AddControllers()");
            sb.I(4).Al(".ConfigureApiBehaviorOptions(options =>");
            sb.I(4).Al("{");
            sb.I(5).Al("options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;");
            sb.I(4).Al("})");

            if(m != null)
            {
                sb.I(4).Al(".AddFluentValidation(options =>");
                sb.I(4).Al("{");
                sb.I(5).Al($"options.RegisterValidatorsFromAssemblyContaining<{m.Model.Name}{m.Behaviour.Name}RequestValidator>();");
                sb.I(4).Al("});");
            }
            sb.B();
            sb.I(3).Al("//todo: Add your service registrations here");
            sb.B();
            sb.I(3).Al("services.AddSwaggerGen(c =>");
            sb.I(3).Al("{");
            sb.I(4).Al($"c.SwaggerDoc(\"v1\", new OpenApiInfo {{ Title = \"{_module.Company} {_module.Project.Singular.Display}\", Version = \"v1\" }});");
            sb.I(3).Al("});");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public void Configure(IApplicationBuilder app, IWebHostEnvironment env)");
            sb.I(2).Al("{");
            sb.I(3).Al("if (env.IsDevelopment())");
            sb.I(3).Al("{");
            sb.I(4).Al("app.UseDeveloperExceptionPage();");
            sb.I(4).Al("app.UseSwagger();");
            sb.I(4).Al($"app.UseSwaggerUI(c => c.SwaggerEndpoint(\"/swagger/v1/swagger.json\", \"{_module.Company} {_module.Project.Singular.Display} v1\"));");
            sb.I(3).Al("}");
            sb.B();
            sb.I(3).Al("app.UseMiddleware<ExceptionHandler>();");
            sb.I(3).Al("app.UseHttpsRedirection();");
            sb.I(3).Al("app.UseRouting();");
            sb.I(3).Al("app.UseAuthorization();");
            sb.I(3).Al("app.UseEndpoints(endpoints =>");
            sb.I(3).Al("{");
            sb.I(4).Al("endpoints.MapControllers();");
            sb.I(3).Al("});");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.B();
            sb.I(1).Al("public class ModelStateValidator");
            sb.I(1).Al("{");
            sb.I(2).Al("public static IActionResult ValidateModelState(ActionContext context)");
            sb.I(2).Al("{");
            sb.I(3).Al("(string fieldName, ModelStateEntry entry) = context.ModelState.First(x => x.Value.Errors.Count > 0);");
            sb.I(3).Al("string errorSerialized = entry.Errors.First().ErrorMessage;");
            sb.B();
            sb.I(3).Al("Error error = Error.Deserialize(errorSerialized);");
            sb.I(3).Al("Envelope envelope = Envelope.Error(error, fieldName);");
            sb.I(3).Al("var envelopeResult = new EnvelopeResult(envelope, HttpStatusCode.BadRequest);");
            sb.B();
            sb.I(3).Al("return envelopeResult;");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            
            return new File("Startup.cs", sb.ToString());
        }
    }
}
