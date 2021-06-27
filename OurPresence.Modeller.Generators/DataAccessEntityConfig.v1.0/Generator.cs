using System;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System.Text;
using OurPresence.Modeller.Domain.Extensions;

namespace DataAccessEntityConfig
{
    public class Generator : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;

        public Generator(ISettings settings, Module module, Model model)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public IOutput Create()
        {
            if(!_model.IsEntity())
                return null;

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Data.EntityMappings");
            sb.Al("{");
            sb.I(1).Al($"public partial class {_model.Name.Singular.Value}Configuration : EntityTypeConfiguration<{_module.Namespace}.Domain.{_model.Name.Singular.Value}>");
            sb.I(1).Al("{");

            sb.I(2).Al($"public override void Configure(EntityTypeBuilder<{_module.Namespace}.Domain.{_model.Name.Singular.Value}> builder)");
            sb.I(2).Al("{");
            sb.I(3).Al("base.Configure(builder);");
            foreach(var field in _model.Fields)
            {
                if(field.DataType == DataTypes.String)
                {
                    sb.I(3).Al($"builder.Property(p => p.{field.Name.Singular.Value})");
                    if(!field.Nullable)
                    {
                        sb.I(4).Al($".IsRequired()");
                    }
                    if(field.MaxLength.HasValue)
                    {
                        sb.I(4).Al($".HasMaxLength({field.MaxLength.Value});");
                    }
                }
            }
            if(_model.HasBusinessKey() != null)
            {
                sb.I(3).Al($"builder.HasIndex(i => i.{_model.HasBusinessKey().Name.Singular.Value}).IsUnique().ForSqlServerIsClustered();");
            }

            if(Settings.SupportRegen)
            {
                sb.I(3).Al("AfterConfigure(builder);");
                sb.I(2).Al("}");
                sb.I(2).Al($"partial void AfterConfigure(EntityTypeBuilder<{_module.Namespace}.Domain.{_model.Name.Singular.Value}> builder);");
            }
            else
            {
                sb.I(2).Al("}");
            }
            sb.I(1).Al("}");
            sb.Al("}");

            return new File($"{_model.Name}Configuration.cs", content: sb.ToString());
        }

        public ISettings Settings { get; }
    }
}
