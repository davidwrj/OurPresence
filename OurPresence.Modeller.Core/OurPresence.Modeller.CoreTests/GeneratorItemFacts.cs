using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using OurPresence.Modeller.Tests.TestFiles;
using FluentAssertions;
using NSubstitute;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace OurPresence.Modeller.CoreTests
{
    public class GeneratorItemFacts
    {
        private string GetThisFilePath()
        {
            return Path.GetFileName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath);
        }

        [Fact]
        public void GeneratorItem_ReturnsValidInstance_WhenInstanceIsCalled()
        {
            var thisTestFilePath = GetThisFilePath();

            var meta = Substitute.For<IMetadata>();
            var settings = Substitute.For<ISettings>();
            var module = new Domain.Module("Company", "Test");

            var sut = new GeneratorItem(meta, thisTestFilePath, typeof(SimpleTestGenerator));

            sut.Instance(settings, module).Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GeneratorItem_ThrowsArgumentException_WhenNoFilePathIsPassed(string filepath)
        {
            var meta = Substitute.For<IMetadata>();
            Action create = () => new GeneratorItem(meta, filepath, typeof(SimpleTestGenerator));

            create.Should().Throw<ArgumentException>().And.Message.Should().Be("File path must be provided (Parameter 'filePath')");
        }

        [Fact]
        public void GeneratorItem_ThrowsArgumentNullException_WhenNoMetadataIsPassed()
        {
            Action create = () => new GeneratorItem(null, GetThisFilePath(), typeof(SimpleTestGenerator));

            create.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("metadata");
        }

        [Fact]
        public void GeneratorItem_ThrowsArgumentNullException_WhenNoGeneratorTypeIsPassed()
        {
            var meta = Substitute.For<IMetadata>();
            Action create = () => new GeneratorItem(meta, GetThisFilePath(), null);

            create.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("generatorType");
        }

        [Fact]
        public void GeneratorItem_FileNotFoundException_WhenInvalidFilePathIsPassed()
        {
            var meta = Substitute.For<IMetadata>();
            Action create = () => new GeneratorItem(meta, "c:\\xx\\invalidFilename.yyy", typeof(SimpleTestGenerator));

            create.Should().Throw<FileNotFoundException>().And.Message.Should().Be("Generator not found.");
        }

        [Fact]
        public void GeneratorItem_Properties_CorrectlySet()
        {
            var thisTestFilePath = GetThisFilePath();

            var meta = Substitute.For<IMetadata>();
            var sut = new GeneratorItem(meta, thisTestFilePath, typeof(SimpleTestGenerator));

            sut.AbbreviatedFileName.Should().NotBeEmpty();
        }
    }
}
