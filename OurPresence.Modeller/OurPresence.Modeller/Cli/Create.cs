using OurPresence.Modeller.Properties;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using NStack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace OurPresence.Modeller.Cli
{
    internal class ListViewItemModel 
    {
        public ListViewItemModel(Model model)
        {
            Model = model;
        }

        public Model Model { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Model.Name);
            sb.Append(Model.IsEntity() ? " (entity)": "");
            return sb.ToString();
        }
    }

    [Command(Name = "create", Description = "Create a new model file")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via reflection")]
    internal class Create
    {
        private readonly ILogger<Program> _logger;
        private readonly IList _models = new List<ListViewItemModel>();

        public Create(ILogger<Program> logger)
        {
            _logger = logger;
        }

        [Argument(1, Description = "The filename for the source model to use during code generation.")]
        public string SourceModel { get; } = string.Empty;

        [Option(Description = "Path to the locally cached generators")]
        [DirectoryExists]
        public string LocalFolder { get; } = Defaults.LocalFolder;

        [Option(Inherited = true, ShortName = "")]
        public bool Overwrite { get; }

        [Option(ShortName = "", Inherited = true)]
        public bool Verbose { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Top level unexpected catch all")]
        internal int OnExecute()
        {
            try
            {
                IGeneratorConfiguration config = new GeneratorConfiguration()
                {
                    Verbose = Verbose,
                    Overwrite = Overwrite
                };

                if (!string.IsNullOrWhiteSpace(LocalFolder))
                    config.LocalFolder = LocalFolder;
                if (!string.IsNullOrWhiteSpace(SourceModel))
                    config.SourceModel = SourceModel;

                _logger.LogTrace(Resources.CreateOnExecture);

                for (var i = 0; i < 15; i++)
                    _models.Add(new ListViewItemModel( new Model($"Model {i}")));

                Application.Init();
                var win = new Window("Module")
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill()
                };
                ShowModule(win);
                Application.Top.Add(win);
                Application.Run();

                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.BuildError, ex, Resources.BuildFailed);
                return 1;
            }
            finally
            {
                _logger.LogTrace(Resources.BuildComplete);
            }
        }

        private void ShowModule(View container)
        {
            var labelIntro = new Label("Fill in the details below to generate a new module file.") { X = 2, Y = 1 };
            var labelCompany = new Label("Company") { X = Pos.Left(labelIntro), Y = Pos.Bottom(labelIntro) + 1 };
            var labelProject = new Label("Project") { X = Pos.Left(labelIntro), Y = Pos.Bottom(labelCompany) + 1 };
            var labelFeature = new Label("Feature (optional)") { X = Pos.Left(labelIntro), Y = Pos.Bottom(labelProject) + 1 };

            var textCompany = new TextField("") { X = Pos.Right(labelFeature) + 1, Y = Pos.Top(labelCompany), Width = 50 };
            var textProject = new TextField("") { X = Pos.Left(textCompany), Y = Pos.Top(labelProject), Width = 50 };
            var textFeature = new TextField("") { X = Pos.Left(textCompany), Y = Pos.Top(labelFeature), Width = 50 };

            var listModels = new ListView(_models) { X = 2, Y = 8, Width = Dim.Percent(95), Height = 10 };

            var ok = new Button("Ok", is_default: true);
            ok.Clicked += () => Application.RequestStop();
            var cancel = new Button("Cancel");
            cancel.Clicked += () => Application.RequestStop();

            var addModel = new Button("Add Model")
            {
                X = Pos.Right(container) - 16,
                Y = Pos.Bottom(container) - 4
            };
            addModel.Clicked += () =>
            {
                var d = new Dialog("Selection Demo", 60, 20, ok, cancel);
                Application.Run(d);
            };

            container.Add(labelIntro, labelCompany, textCompany, labelProject, textProject, labelFeature, textFeature, addModel, listModels);
        }
    }
}
