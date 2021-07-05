// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurPresence.Modeller.Generator
{
    public class Presenter : IPresenter
    {
        private readonly ILoader<IEnumerable<IGeneratorItem>> _generatorLoader;
        private readonly ILogger<IPresenter> _logger;

        public Presenter(IGeneratorConfiguration generatorConfiguration, ILoader<IEnumerable<IGeneratorItem>> generatorLoader, ILogger<IPresenter> logger)
        {
            GeneratorConfiguration = generatorConfiguration ?? throw new ArgumentNullException(nameof(generatorConfiguration));
            _generatorLoader = generatorLoader ?? throw new ArgumentNullException(nameof(generatorLoader));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IGeneratorConfiguration GeneratorConfiguration { get; }

        public void Display()
        {
            var folder = System.IO.Path.Combine(GeneratorConfiguration.LocalFolder, GeneratorConfiguration.Target);
            _logger.LogInformation("Available generators");
            _logger.LogInformation($"  location: {folder}");

            var table = Process(folder);
            var sb = new StringBuilder();
            sb.AppendLine();
            foreach (var row in table)
            {
                foreach (var cell in row)
                    sb.Append(cell);
                sb.AppendLine();
            }
            _logger.LogInformation(sb.ToString());
        }

        private IEnumerable<IEnumerable<string>> Process(string folder)
        {
            var rows = new List<List<string>>();

            if (!_generatorLoader.TryLoad(folder, out var generators))
                return rows;

            foreach (var generator in generators)
            {
                var (filename, version) = FileHelper.GetAbbreviatedFilename(generator.FilePath);
                var m = generator.Metadata;
                var vers = m.Version == null ? new GeneratorVersion().ToString() : m.Version.ToString();
                if (GeneratorConfiguration.Verbose)
                {
                    var row = new List<string> { filename, m.Name, vers };
                    if (!string.IsNullOrWhiteSpace(m.Description))
                        row.Add(m.Description);
                    rows.Add(row);
                    foreach (var item in generator.Metadata.SubGenerators)
                        rows.Add(new List<string> { "", item.Name });
                }
                else
                    rows.Add(new List<string> { filename, m.Name, vers });
            }

            if (rows.Any())
            {
                var cols = GeneratorConfiguration.Verbose ? 3 : 2;
                var widths = new List<int>(cols);
                for (var col = 0; col < cols; col++)
                    widths.Add(rows.Max(l => l[col].Length));

                foreach (var row in rows)
                {
                    for (var col = 0; col < cols; col++)
                        row[col] = row[col].PadRight(widths[col]) + " | ";
                }
            }

            return rows;
        }
    }
}
