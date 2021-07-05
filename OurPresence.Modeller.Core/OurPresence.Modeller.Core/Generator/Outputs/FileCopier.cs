// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace OurPresence.Modeller.Generator.Outputs
{
    public class FileCopier : IFileCreator
    {
        private readonly ILogger<IFileCreator> _logger;

        public FileCopier(ILogger<IFileCreator> logger)
        {
            _logger = logger;
        }

        public Type SupportedType => typeof(IFileCopy);

        public void Create(IOutput output)
        {
            Create(output, null, false);
        }
        
        public void Create(IOutput output, string? path, bool overwrite)
        {
            if (output is not IFileCopy fileCopy)
                throw new NotSupportedException($"{nameof(CreateFile)} only supports {SupportedType.FullName} output types.");

            var basePath = path;
            if (basePath != null && !System.IO.Path.IsPathRooted(fileCopy.Destination))
            {
                fileCopy.Destination = !string.IsNullOrWhiteSpace(fileCopy.Destination) ? System.IO.Path.Combine(basePath, fileCopy.Destination) : basePath;
            }

            try
            {
                if (!overwrite && System.IO.File.Exists(fileCopy.Destination))
                    _logger.LogInformation($"Copy: {fileCopy.Source} skipped.");
                else
                {
                    System.IO.File.Copy(fileCopy.Source, fileCopy.Destination, overwrite);
                    _logger.LogInformation($"Copy: {fileCopy.Source} -> {fileCopy.Destination} - success");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Copy: {fileCopy.Source} -> {fileCopy.Destination} - failed.");
            }
        }
    }
}
