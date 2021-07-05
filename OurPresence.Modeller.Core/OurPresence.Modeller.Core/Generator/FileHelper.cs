// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;

namespace OurPresence.Modeller.Generator
{
    public static class FileHelper
    {
        public static (string filename, GeneratorVersion version) GetAbbreviatedFilename(string filePath)
        {
            var filename = Path.GetFileNameWithoutExtension(filePath);
            var parts = filename.Split('.');
            if (parts.Any())
            {
                var f = string.Empty;
                var v = string.Empty;
                for (var i = 0; i < parts.Length; i++)
                {
                    if (parts[i].StartsWith("v", StringComparison.InvariantCultureIgnoreCase) &&
                        int.TryParse(parts[i].Substring(1), out var number1))
                        v += number1.ToString() + ".";
                    else if (int.TryParse(parts[i], out var number2))
                        v += number2.ToString() + ".";
                    else
                        f += parts[i] + ".";
                }

                var fn = string.IsNullOrEmpty(f) ? string.Empty : f.Substring(0, f.Length - 1);
                var ve = string.IsNullOrEmpty(v)
                    ? new GeneratorVersion()
                    : new GeneratorVersion(v.Substring(0, v.Length - 1));
                return (fn, ve);
            }

            return (filename, new GeneratorVersion());
        }

        public static bool UpdateLocalGenerators(string? serverFolder = null, string? localFolder = null,
            bool overwrite = false, Action<string>? output = null)
        {
            if (string.IsNullOrWhiteSpace(serverFolder))
                return false;
            // if (string.IsNullOrWhiteSpace(localFolder))
            //     localFolder = Defaults.LocalFolder;

            var server = new DirectoryInfo(serverFolder);
            var local = new DirectoryInfo(localFolder);

            if (!server.Exists)
                return false;

            DirectoryCopy(server, local, true, overwrite, output);
            return true;
        }

        private static void DirectoryCopy(DirectoryInfo sourceDirName, DirectoryInfo destDirName, bool copySubDirs,
            bool overwrite, Action<string>? output)
        {
            if (!sourceDirName.Exists)
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " +
                                                     sourceDirName);

            var dirs = sourceDirName.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!destDirName.Exists)
            {
                output?.Invoke($"creating {destDirName.FullName}");
                destDirName.Create();
            }

            // Get the files in the directory and copy them to the new location.
            var files = sourceDirName.GetFiles();
            foreach (var file in files)
            {
                var tempPath = Path.Combine(destDirName.FullName, file.Name);
                if (System.IO.File.Exists(tempPath) && !overwrite)
                {
                    continue;
                }

                output?.Invoke($"copying {file.Name} to {destDirName.Name}");
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (var subDir in dirs)
                {
                    var tempPath = new DirectoryInfo(Path.Combine(destDirName.FullName, subDir.Name));
                    DirectoryCopy(subDir, tempPath, copySubDirs, overwrite, output);
                }
            }
        }
    }
}