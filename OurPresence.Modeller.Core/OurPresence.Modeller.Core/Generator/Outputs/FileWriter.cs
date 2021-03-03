using OurPresence.Modeller.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace OurPresence.Modeller.Generator.Outputs
{
    public class FileWriter : IFileWriter
    {
        private readonly ILogger<FileWriter> _logger;

        public FileWriter(ILogger<FileWriter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Write(IFile file)
        {
            if (file == null) return;

            _logger.LogInformation($"File write: {file.FullName}");

            var fileInfo = new FileInfo(file.FullName);
            if (fileInfo.Exists && !file.CanOverwrite) return;

            var dir = new DirectoryInfo(file.Path);
            if (!dir.Exists)
                Directory.CreateDirectory(dir.FullName);

            System.IO.File.WriteAllText(file.FullName, file.Content);
        }
    }
}
