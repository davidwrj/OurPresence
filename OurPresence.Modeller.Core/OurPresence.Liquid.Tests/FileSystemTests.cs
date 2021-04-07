using System.Globalization;
using OurPresence.Liquid.Exceptions;
using OurPresence.Liquid.FileSystems;
using NUnit.Framework;

namespace OurPresence.Liquid.Tests
{
    using System.Reflection;

    [TestFixture]
    public class FileSystemTests
    {
        [Test]
        public void TestDefault()
        {
            Assert.Throws<FileSystemException>(() => new BlankFileSystem().ReadTemplateFile(new Context(CultureInfo.InvariantCulture), "dummy"));
        }


        [Test]
        [Category("windows")]
        public void TestLocal()
        {
            LocalFileSystem fileSystem = new LocalFileSystem(@"D:\Some\Path");
            Assert.AreEqual(@"D:\Some\Path\_mypartial.liquid", fileSystem.FullPath("mypartial"));
            Assert.AreEqual(@"D:\Some\Path\dir\_mypartial.liquid", fileSystem.FullPath("dir/mypartial"));

            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("../dir/mypartial"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("/dir/../../dir/mypartial"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("/etc/passwd"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath(@"C:\mypartial"));
        }

        [Test]
        [Category("windows")]
        public void TestLocalWithBracketsInPath()
        {
            LocalFileSystem fileSystem = new LocalFileSystem(@"D:\Some (thing)\Path");
            Assert.AreEqual(@"D:\Some (thing)\Path\_mypartial.liquid", fileSystem.FullPath("mypartial"));
            Assert.AreEqual(@"D:\Some (thing)\Path\dir\_mypartial.liquid", fileSystem.FullPath("dir/mypartial"));
        }


        [Test]
        public void TestEmbeddedResource()
        {
            var assembly = typeof(FileSystemTests).GetTypeInfo().Assembly;
            EmbeddedFileSystem fileSystem = new EmbeddedFileSystem(assembly, "OurPresence.Liquid.Tests.Embedded");
            Assert.AreEqual(@"OurPresence.Liquid.Tests.Embedded._mypartial.liquid", fileSystem.FullPath("mypartial"));
            Assert.AreEqual(@"OurPresence.Liquid.Tests.Embedded.dir._mypartial.liquid", fileSystem.FullPath("dir/mypartial"));

            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("../dir/mypartial"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("/dir/../../dir/mypartial"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath("/etc/passwd"));
            Assert.Throws<FileSystemException>(() => fileSystem.FullPath(@"C:\mypartial"));
        }
    }
}
