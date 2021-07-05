// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Generator.Outputs;
using OurPresence.Modeller.Interfaces;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace OurPresence.Modeller.CoreTests
{
    public class OutputStrategyFacts
    {
        [Fact]
        public void OutputStrategy_Returns_Expected()
        {
            var mockFileWriter = Substitute.For<IFileWriter>();
            var fileWriter = mockFileWriter;
            var strategies = new List<IFileCreator> { new CreateSnippet(fileWriter), new CreateProject(fileWriter) };
            var config = Substitute.For<IGeneratorConfiguration>();
            config.OutputPath.Returns(System.IO.Path.GetTempPath());
            var snippet = new Snippet("MySnippet", "My Content " + Guid.NewGuid().ToString("N"));

            var outputStrategy = new OutputStrategy(strategies);

            outputStrategy.Create(snippet, path: config.OutputPath);
        }
    }
}
