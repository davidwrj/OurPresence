using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Modeller.Loaders
{
    public class GeneratorLoader : ILoader<IEnumerable<IGeneratorItem>>
    {
        private class TempGeneratorDetail : IMetadata
        {
            public TempGeneratorDetail(string name, string description, Type entryPoint, IEnumerable<Type> subGenerators, IGeneratorVersion version)
            {
                Name = name;
                Description = description;
                EntryPoint = entryPoint;
                SubGenerators = subGenerators;
                Version = version;
            }
            public string Name { get; }
            public string Description { get; }
            public Type EntryPoint { get; }
            public IEnumerable<Type> SubGenerators { get; }
            public bool IsAlphaRelease { get; }
            public bool IsBetaRelease { get; }
            public IGeneratorVersion Version { get; }
        }

        private IEnumerable<IGeneratorItem> Process(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                filePath = Defaults.LocalFolder;

            var local = new System.IO.DirectoryInfo(filePath);
            var list = new List<IGeneratorItem>();
            if (!local.Exists)
                return list;

            AddFiles(list, local);
            return list;
        }

        private static void AddFiles(List<IGeneratorItem> list, System.IO.DirectoryInfo folder)
        {
            foreach (var subFolder in folder.GetDirectories())
            {
                AddFiles(list, subFolder);
            }

            var asmLoader = new AssemblyLoader(folder.FullName);
            foreach (var file in folder.GetFiles("*.dll"))
            {
                var deps = file.FullName.Substring(0, file.FullName.Length - 3) + "deps.json";
                if (!System.IO.File.Exists(deps))
                    continue;

                var ass = asmLoader.Load(file.FullName);
                var dt = ass.DefinedTypes;
                var metaDataTypes = dt.Where(t => t.ImplementedInterfaces.Any(it => it.Name == "IMetadata"));
                foreach (var type in metaDataTypes)
                {
                    if (type.IsAbstract || type.IsInterface || !type.IsPublic)
                        continue;

                    var obj = Activator.CreateInstance(type);
                    if (obj == null)
                        continue;

                    if (obj is IMetadata instance)
                    {
                        var entryPoint = instance.EntryPoint;
                        if (entryPoint == null)
                        {
                            continue;
                        }
                        list.Add(new GeneratorItem(instance, file.FullName, entryPoint));
                    }
                    else
                    {
                        var name = type.GetProperty("Name")?.GetValue(obj)?.ToString();
                        var entryPoint = type.GetProperty("EntryPoint")?.GetValue(obj) as Type;
                        if (string.IsNullOrEmpty(name) || entryPoint == null)
                            continue;

                        var description = type.GetProperty("Description")?.GetValue(obj)?.ToString() ?? string.Empty;
                        var subGenerators = type.GetProperty("SubGenerators")?.GetValue(obj) as IEnumerable<Type> ?? new List<Type>();
                        var version = type.GetProperty("Version")?.GetValue(obj) as IGeneratorVersion ?? new GeneratorVersion();

                        var md = new TempGeneratorDetail(name, description, entryPoint, subGenerators, version);
                        list.Add(new GeneratorItem(md, file.FullName, entryPoint));
                    }
                }
            }
        }

        IEnumerable<IGeneratorItem> ILoader<IEnumerable<IGeneratorItem>>.Load(string filePath)
        {
            return Process(filePath);
        }

        bool ILoader<IEnumerable<IGeneratorItem>>.TryLoad(string filePath, out IEnumerable<IGeneratorItem> generators)
        {
            try
            {
                generators = Process(filePath);
                return generators.Any();
            }
            catch
            {
                generators = new List<IGeneratorItem>();
                return false;
            }
        }
    }
}
