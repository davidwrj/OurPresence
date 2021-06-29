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
    internal class Create
    {
        private readonly ILogger<Create> _logger;
        private readonly IList _models = new List<ListViewItemModel>();

        public Create(ILogger<Create> logger, IConfiguration configuration)
        {
            _logger = logger;
            LocalFolder = configuration["LocalFolder"];
        }

        [Argument(1, Description = "The filename for the source model to use during code generation.")]
        public string SourceModel { get; } = string.Empty;

        [Option(Description = "Path to the locally cached generators")]
        [DirectoryExists]
        public string LocalFolder { get; init; }

        [Option(Inherited = true, ShortName = "")]
        public bool Overwrite { get; }

        [Option(ShortName = "", Inherited = true)]
        public bool Verbose { get; }

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

                _logger.LogTrace("Create Command - OnExecute");

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
                _logger.LogError(LoggingEvents.BuildError, ex, "Build command failed");
                return 1;
            }
            finally
            {
                _logger.LogTrace("Generator Build Command - complete");
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
            ok.Clicked += Application.RequestStop;
            var cancel = new Button("Cancel");
            cancel.Clicked += Application.RequestStop;

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
