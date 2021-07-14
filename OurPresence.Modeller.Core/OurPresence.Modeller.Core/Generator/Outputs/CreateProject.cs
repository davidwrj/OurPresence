// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;
using System;

namespace OurPresence.Modeller.Generator.Outputs
{
    public class CreateProject : IFileCreator
    {
        private readonly IFileWriter _fileWriter;

        public CreateProject(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
        }

        public Type SupportedType => typeof(IProject);

        public void Create(IOutput output, string? path = null, bool overwrite = false)
        {
            if (!(output is IProject project))
                throw new NotSupportedException($"{nameof(CreateProject)} only supports {SupportedType.FullName} output types.");

            var p = System.IO.Path.IsPathRooted(project.Path)
                ? project.Path
                : !string.IsNullOrWhiteSpace(path) ? System.IO.Path.Combine(path, project.Path) : project.Path;

            foreach (var fg in project.FileGroups)
            {
                var groupPath = string.IsNullOrWhiteSpace(fg.Name) ? p : System.IO.Path.Combine(p, fg.Name);
                foreach (var file in fg.Files)
                {
                    if(!string.IsNullOrWhiteSpace(file.Path))
                    {
                        var dirs = file.Path.Split(System.IO.Path.DirectorySeparatorChar);
                        if(dirs.Length >= 1)
                        {
                            var pos = groupPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                            if (pos >= 0)
                            {
                                var lastDir = groupPath.Substring(pos + 1);
                                if(dirs[0] == lastDir)
                                {
                                    file.Path=file.Path.Substring(lastDir.Length).Trim(System.IO.Path.DirectorySeparatorChar);
                                }
                            }
                        }
                    }
                    var filePath = (string.IsNullOrWhiteSpace(file.Path) || groupPath.Contains(file.Path)) ? groupPath : System.IO.Path.Combine(groupPath, file.Path);
                    file.Path = filePath;

                    var fileOutput = new CreateFile(_fileWriter);
                    fileOutput.Create(file, path, overwrite);
                }
            }
        }
    }
}
