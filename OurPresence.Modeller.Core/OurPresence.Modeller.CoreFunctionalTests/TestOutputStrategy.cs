using OurPresence.Modeller.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Modeller.CoreFunctionalTests
{
    public class TestOutputStrategy : IOutputStrategy
    {
        private List<string> _projectFiles = new List<string>();

        public void Create(IOutput output, IGeneratorConfiguration generatorConfiguration)
        {
            if (output is ISolution solution)
            {
                if (solution.Directory != null && !System.IO.Path.IsPathRooted(solution.Directory))
                    solution.Directory = System.IO.Path.Combine(generatorConfiguration.OutputPath, solution.Directory);

                foreach (var project in solution.Projects)
                {
                    var path = System.IO.Path.IsPathRooted(project.Path) ? project.Path : System.IO.Path.Combine(generatorConfiguration.OutputPath, project.Path);
                    foreach (var fg in project.FileGroups)
                    {
                        var groupPath = string.IsNullOrWhiteSpace(fg.Name) ? path : System.IO.Path.Combine(path, fg.Name);
                        foreach (var file in fg.Files.Where(f => f.Name.EndsWith(".csproj")))
                        {
                            file.Path = (string.IsNullOrWhiteSpace(file.Path) || groupPath.Contains(file.Path)) ? groupPath : System.IO.Path.Combine(groupPath, file.Path);
                            if (!System.IO.Path.IsPathRooted(file.Path))
                                file.Path = System.IO.Path.Combine(generatorConfiguration.OutputPath, file.Path);
                            _projectFiles.Add(file.FullName);
                        }
                    }
                }
            }
        }

        public void Create(IOutput output, string path = null, bool overwrite = false)
        {
            if (output is ISolution solution)
            {
                if (solution.Directory != null && !System.IO.Path.IsPathRooted(solution.Directory))
                    solution.Directory = System.IO.Path.Combine(path, solution.Directory);

                foreach (var project in solution.Projects)
                {
                    var rootPath = System.IO.Path.IsPathRooted(project.Path) ? project.Path : System.IO.Path.Combine(path, project.Path);
                    foreach (var fg in project.FileGroups)
                    {
                        var groupPath = string.IsNullOrWhiteSpace(fg.Name) ? rootPath : System.IO.Path.Combine(rootPath, fg.Name);
                        foreach (var file in fg.Files.Where(f => f.Name.EndsWith(".csproj")))
                        {
                            file.Path = (string.IsNullOrWhiteSpace(file.Path) || groupPath.Contains(file.Path)) ? groupPath : System.IO.Path.Combine(groupPath, file.Path);
                            if (!System.IO.Path.IsPathRooted(file.Path))
                                file.Path = System.IO.Path.Combine(path, file.Path);
                            _projectFiles.Add(file.FullName);
                        }
                    }
                }
            }
        }

        public IEnumerable<string> ProjectFiles => _projectFiles;
    }
}
