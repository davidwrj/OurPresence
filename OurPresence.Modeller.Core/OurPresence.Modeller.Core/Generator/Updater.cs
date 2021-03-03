using OurPresence.Modeller.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace OurPresence.Modeller.Generator
{
    public class Updater : IUpdater
    {
        private readonly IGeneratorConfiguration _generatorConfiguration;
        private readonly ILogger<IUpdater> _logger;
        private int _affected;

        IGeneratorConfiguration IUpdater.GeneratorConfiguration => _generatorConfiguration;

        public Updater(IGeneratorConfiguration generatorConfiguration, ILogger<IUpdater> logger)
        {
            _generatorConfiguration = generatorConfiguration ?? throw new ArgumentNullException(nameof(generatorConfiguration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        void IUpdater.Refresh()
        {
            var folder = System.IO.Path.Combine(_generatorConfiguration.LocalFolder, _generatorConfiguration.Target);

            _affected = 0;
            _logger.LogInformation($"Updating generator files for target: {_generatorConfiguration.Target}");
            _logger.LogInformation($"Server Folder: {_generatorConfiguration.ServerFolder}");
            _logger.LogInformation($"Local Folder: {folder}");
            _logger.LogInformation($"Overwrite: {_generatorConfiguration.Overwrite}");

            if (UpdateLocalGenerators())
            {
                _logger.LogInformation($"Update completed successfully. Files affected: {_affected}");
            }
            else
            {
                _logger.LogInformation($"Update failed. Files affected: {_affected}");
            }
        }

        private bool UpdateLocalGenerators()
        {
            var server = new System.IO.DirectoryInfo(_generatorConfiguration.ServerFolder);
            var folder = System.IO.Path.Combine(_generatorConfiguration.LocalFolder, _generatorConfiguration.Target);
            var local = new System.IO.DirectoryInfo(folder);

            if (!server.Exists)
            {
                _logger.LogWarning($"Server Folder '{server.FullName}' not found.");
                return false;
            }

            DirectoryCopy(server, local);
            return true;
        }

        private void DirectoryCopy(System.IO.DirectoryInfo sourceDirName, System.IO.DirectoryInfo destDirName)
        {
            if (!sourceDirName.Exists)
            {
                return;
            }

            var dirs = sourceDirName.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!destDirName.Exists)
            {
                if (_generatorConfiguration.Verbose)
                    _logger.LogInformation($"creating {destDirName.FullName}");
                destDirName.Create();
            }

            // Get the files in the directory and copy them to the new location.
            var files = sourceDirName.GetFiles();
            foreach (var file in files)
            {
                var temppath = System.IO.Path.Combine(destDirName.FullName, file.Name);
                if (System.IO.File.Exists(temppath) && !_generatorConfiguration.Overwrite)
                {
                    if (_generatorConfiguration.Verbose)
                        _logger.LogInformation($"skipping {file.Name}");
                    continue;
                }

                if (_generatorConfiguration.Verbose)
                    _logger.LogInformation($"copying {file.Name} to {destDirName.Name}");
                file.CopyTo(temppath, _generatorConfiguration.Overwrite);
                _affected++;
            }

            // copy Sub-directories and their contents to new location.
            foreach (var subdir in dirs)
            {
                var temppath = new System.IO.DirectoryInfo(System.IO.Path.Combine(destDirName.FullName, subdir.Name));
                DirectoryCopy(subdir, temppath);
            }
        }
    }
}
