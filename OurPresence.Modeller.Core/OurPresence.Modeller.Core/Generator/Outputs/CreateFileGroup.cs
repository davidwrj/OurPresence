// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;
using System;

namespace OurPresence.Modeller.Generator.Outputs
{
    public class CreateFileGroup : IFileCreator
    {
        private readonly IFileWriter _fileWriter;

        public CreateFileGroup(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
        }

        public Type SupportedType => typeof(IFileGroup);

        public void Create(IOutput output, string? path = null, bool overwrite = false)
        {
            if (!(output is IFileGroup fileGroup))
            {
                throw new NotSupportedException($"{nameof(CreateFileGroup)} only supports {SupportedType.FullName} output types.");
            }

            if (string.IsNullOrWhiteSpace(output.Name) && !string.IsNullOrWhiteSpace(path))
            {
                fileGroup.SetPath(path);
            }
            else if (!System.IO.Path.IsPathRooted(output.Name))
            {
                if (!string.IsNullOrWhiteSpace(path))
                {
                    fileGroup.SetPath(System.IO.Path.Combine(path, output.Name));
                }
                else
                {
                    fileGroup.SetPath(output.Name);
                }
            }
            foreach (var file in fileGroup.Files)
            {
                var fileOutput = new CreateFile(_fileWriter);
                fileOutput.Create(file, path, overwrite);
            }
        }
    }
}
