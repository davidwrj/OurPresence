// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;
using System;
using System.IO;

namespace OurPresence.Modeller.Generator
{
    public class GeneratorItem : IGeneratorItem
    {
        private IGenerator _instance;

        public GeneratorItem(IMetadata metadata, string filePath, Type generatorType)
        {
            Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            Type = generatorType ?? throw new ArgumentNullException(nameof(generatorType));

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path must be provided", nameof(filePath));
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException("Generator not found.", filePath);

            FilePath = filePath;
            var (filename, _) = FileHelper.GetAbbreviatedFilename(filePath);
            AbbreviatedFileName = filename;
        }

        public string AbbreviatedFileName { get; }

        public IMetadata Metadata { get; }

        public Type Type { get; }

        public string FilePath { get; }

        public IGenerator Instance(params object[] args)
        {
            if (_instance == null)
            {
                if (Activator.CreateInstance(Type, args) is IGenerator g)
                    _instance = g;
                if (_instance == null)
                    throw new NullReferenceException($"Unable to create the {Metadata.Name} generator");
            }
            return _instance;
        }
    }
}
