using System.Globalization;
using OurPresence.Modeller.Liquid.Exceptions;
using OurPresence.Modeller.Liquid.FileSystems;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    using System.Reflection;

    public class FileSystemTests
    {
        [Fact]
        public void TestDefault()
        {
            Assert.Throws<FileSystemException>(() => new BlankFileSystem().ReadTemplateFile(new Context(CultureInfo.InvariantCulture), "dummy"));
        }
        

        [Fact]        
        [Trait("environment","windows")]
        public void TestLocal()
        {
            LocalFileSystem fileSystem = new LocalFileSystem(@"D:\Some\Path");
            Assert.Equal(@"D:\Some\Path\_mypartial.liquid", fileSystem.FullPath("mypartial"));
            Assert.Equal(@"D:\Some\Path\dir\_mypartial.liquid", fileSystem.FullPath("dir/mypartial"));

            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("../dir/mypartial"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("/dir/../../dir/mypartial"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("/etc/passwd"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath(@"C:\mypartial"));
        }

        [Fact]
        [Trait("environment", "windows")]
        public void TestLocalWithBracketsInPath()
        {
            LocalFileSystem fileSystem = new LocalFileSystem(@"D:\Some (thing)\Path");
            Assert.Equal(@"D:\Some (thing)\Path\_mypartial.liquid", fileSystem.FullPath("mypartial"));
            Assert.Equal(@"D:\Some (thing)\Path\dir\_mypartial.liquid", fileSystem.FullPath("dir/mypartial"));
        }
        

        [Fact]
        public void TestEmbeddedResource()
        {
            var assembly = typeof(FileSystemTests).GetTypeInfo().Assembly;
            EmbeddedFileSystem fileSystem = new EmbeddedFileSystem(assembly, "OurPresence.Modeller.Liquid.Tests.Embedded");
            Assert.Equal(@"OurPresence.Modeller.Liquid.Tests.Embedded._mypartial.liquid", fileSystem.FullPath("mypartial"));
            Assert.Equal(@"OurPresence.Modeller.Liquid.Tests.Embedded.dir._mypartial.liquid", fileSystem.FullPath("dir/mypartial"));

            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("../dir/mypartial"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("/dir/../../dir/mypartial"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("/etc/passwd"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath(@"C:\mypartial"));
        }
    }
}
