// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using McMaster.NETCore.Plugins;
using System.Reflection;

namespace OurPresence.Modeller.Loaders
{
    internal class AssemblyLoader
    {
        private readonly string _folderPath;

        public AssemblyLoader(string folderPath)
        {
            _folderPath = folderPath;
        }

        private System.IO.FileInfo LoadFileInfo(string assemblyName) => new System.IO.FileInfo(System.IO.Path.Combine(_folderPath, $"{assemblyName}.dll"));

        internal Assembly Load(string filePath)
        {
            var loader = PluginLoader.CreateFromAssemblyFile(filePath, sharedTypes: new[] { typeof(ISettings), typeof(Domain.Module) });
            return loader.LoadDefaultAssembly();
        }

        internal Assembly? Load(AssemblyName assemblyName)
        {
            if (assemblyName == null || string.IsNullOrWhiteSpace(assemblyName.Name)) return null;
            var fileInfo = LoadFileInfo(assemblyName.Name);
            return System.IO.File.Exists(fileInfo.FullName)
                ? TryGetAssemblyFromAssemblyName(assemblyName, out var assembly) ? assembly : Load(fileInfo.FullName)
                : Assembly.Load(assemblyName);
        }

        private bool TryGetAssemblyFromAssemblyName(AssemblyName assemblyName, out Assembly? assembly)
        {
            if (assemblyName == null || string.IsNullOrWhiteSpace(assemblyName.Name))
            {
                assembly = null;
                return false;
            }

            try
            {
                var fileInfo = LoadFileInfo(assemblyName.Name);

                var loader = PluginLoader.CreateFromAssemblyFile(fileInfo.FullName, sharedTypes: new[] { typeof(ISettings), typeof(IMetadata), typeof(IGenerator), typeof(IOutput), typeof(GeneratorVersion) });
                assembly = loader.LoadDefaultAssembly();
                return true;
            }
            catch
            {
                assembly = null;
                return false;
            }
        }
    }
}
