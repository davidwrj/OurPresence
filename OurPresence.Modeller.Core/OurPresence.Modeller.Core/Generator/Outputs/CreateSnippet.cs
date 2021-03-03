using OurPresence.Modeller.Interfaces;
using System;
using System.Linq;

namespace OurPresence.Modeller.Generator.Outputs
{
    public class CreateSnippet : IFileCreator
    {
        private readonly IFileWriter _fileWriter;

        public CreateSnippet(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
        }

        public Type SupportedType => typeof(ISnippet);

        public void Create(IOutput output, string path=null, bool overwrite = false)
        {
            var basePath = path;
            if (!(output is ISnippet snippet))
                throw new NotSupportedException($"CreateSnippet only supports {SupportedType.FullName} output types.");

            var filename = snippet.Name ?? "MySnippet";
            var files = System.IO.Directory.GetFiles(basePath, filename + "*.txt");
            if (files.Any())
            {
                var max = 1;
                foreach (var file in files)
                {
                    var name = System.IO.Path.GetFileNameWithoutExtension(file).Substring(filename.Length);
                    if (int.TryParse(name, out var number))
                    {
                        max = Math.Max(max, number);
                    }
                }
                filename += max.ToString();
            }

            var ext = System.IO.Path.GetExtension(filename);
            if (string.IsNullOrEmpty(ext) || ext != ".txt")
                filename += ".txt";

            var fileToOutput = new File(filename, snippet.Content)
            {
                CanOverwrite = true
            };
            _fileWriter.Write(fileToOutput);
        }
    }
}
