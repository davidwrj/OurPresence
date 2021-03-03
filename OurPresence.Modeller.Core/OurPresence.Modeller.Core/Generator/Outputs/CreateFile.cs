using OurPresence.Modeller.Interfaces;
using System;

namespace OurPresence.Modeller.Generator.Outputs
{
    public class CreateFile : IFileCreator
    {
        private readonly IFileWriter _fileWriter;

        public CreateFile(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
        }

        public Type SupportedType => typeof(IFile);

        public void Create(IOutput output, string? path = null, bool overwrite = false)
        {
            if (output is not IFile file)
                throw new NotSupportedException($"{nameof(CreateFile)} only supports {SupportedType.FullName} output types.");

            if (string.IsNullOrWhiteSpace(file.Path) && !string.IsNullOrWhiteSpace(path) || path is null)
                file.Path = path;
            else if (!System.IO.Path.IsPathRooted(file.Path))
                file.Path = System.IO.Path.Combine(path, file.Path);

            _fileWriter.Write(file);
        }
    }
}
