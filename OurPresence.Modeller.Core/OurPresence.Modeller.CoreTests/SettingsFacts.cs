using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using FluentAssertions;
using Xunit;
using NSubstitute;

namespace OurPresence.Modeller.CoreTests
{
    public class SettingsFacts
    {
        [Fact]
        public void Setting_SupportRegen_DefaultstoTrue()
        {
            var context = Substitute.For<IGeneratorConfiguration>();
            context.Version.Returns(new GeneratorVersion());

            ISettings sut = new Settings(context);
            sut.SupportRegen.Should().BeTrue();
        }

        //[Fact]
        //public void Setting_GetPackageVersion_ReturnsContextPackageVersion()
        //{
        //    var context = new Mock<IGeneratorConfiguration>();
        //    context.Setup(c => c.Packages).Returns(new List<IPackage> { new Package("Package1", "1.2.3") });
        //    context.Setup(c => c.Version).Returns(new GeneratorVersion());

        //    ISettings sut = new Settings(context.Object);
        //    sut.GetPackageVersion("Package1").Should().Be("1.2.3");
        //}

        [Fact]
        public void Setting_GetPackageVersion_ReturnsEmptyString_WhenNoPackages()
        {
            var context = Substitute.For<IGeneratorConfiguration>();
            context.Version.Returns(new GeneratorVersion());

            ISettings sut = new Settings(context);
            sut.GetPackageVersion("Package1","1.0.0").Should().Be("1.0.0");
        }

        //[Fact]
        //public void Setting_RegisterPackageViaObject_Succeeds()
        //{
        //    var packages = new List<IPackage>();
        //    var context = new Mock<IGeneratorConfiguration>();
        //    context.Setup(c => c.Packages).Returns(packages);
        //    context.Setup(c => c.Version).Returns(new GeneratorVersion());

        //    ISettings sut = new Settings(context.Object);
        //    sut.RegisterPackage(new Package("Package1", "1.2.3"));

        //    sut.GetPackageVersion("Package1").Should().Be("1.2.3");
        //}

        //[Fact]
        //public void Setting_RegisterNullPackageViaObject_DoesNotError()
        //{
        //    var packages = new List<IPackage>();
        //    var context = new Mock<IGeneratorConfiguration>();
        //    context.Setup(c => c.Packages).Returns(packages);
        //    context.Setup(c => c.Version).Returns(new GeneratorVersion());

        //    ISettings sut = new Settings(context.Object);
        //    sut.RegisterPackage(null);

        //    sut.PackagesInitialised().Should().BeFalse();
        //}

        //[Fact]
        //public void Setting_RegisterPackageViaName_Succeeds()
        //{
        //    var packages = new List<IPackage>();
        //    var context = new Mock<IGeneratorConfiguration>();
        //    context.Setup(c => c.Packages).Returns(packages);
        //    context.Setup(c => c.Version).Returns(new GeneratorVersion());

        //    ISettings sut = new Settings(context.Object);
        //    sut.RegisterPackage("Package1", "1.2.3");

        //    sut.GetPackageVersion("Package1").Should().Be("1.2.3");
        //}

        //[Fact]
        //public void Setting_RegisterPackageViaCollection_Succeeds()
        //{
        //    var packages = new List<IPackage>();
        //    var newPackages = new List<Package> { new Package("Package1", "1.2.3"), new Package("name", "1.0") };
        //    var context = new Mock<IGeneratorConfiguration>();
        //    context.Setup(c => c.Packages).Returns(packages);
        //    context.Setup(c => c.Version).Returns(new GeneratorVersion());

        //    ISettings sut = new Settings(context.Object);
        //    sut.RegisterPackages(newPackages);

        //    sut.GetPackageVersion("Package1").Should().Be("1.2.3");
        //    sut.GetPackageVersion("name").Should().Be("1.0");
        //}

        //[Theory]
        //[InlineData("", "1.2.3")]
        //[InlineData("name", "")]
        //[InlineData(null, "1.2.3")]
        //[InlineData("name", null)]
        //public void Setting_RegisterPackageViaName_DoesNotErrorWhenNameIsNull(string name, string version)
        //{
        //    var packages = new List<IPackage>();
        //    var context = new Mock<IGeneratorConfiguration>();
        //    context.Setup(c => c.Packages).Returns(packages);
        //    context.Setup(c => c.Version).Returns(new GeneratorVersion());

        //    ISettings sut = new Settings(context.Object);
        //    sut.RegisterPackage(name, version);

        //    sut.PackagesInitialised().Should().BeFalse();
        //}

        //[Theory]
        //[InlineData("1.0", "2.0")]
        //[InlineData("2.0", "2.0")]
        //[InlineData("2.1.0", "2.1.0")]
        //[InlineData("3.0", "3.0")]
        //[InlineData(null, "2.0")]
        //[InlineData("something", "2.0")]
        //public void Setting_RegisterExistingPackage_OnlyUpdatesToLatestVersion(string version, string expected)
        //{
        //    var packages = new List<IPackage> { new Package("name", "2.0") };
        //    var context = new Mock<IGeneratorConfiguration>();
        //    context.Setup(c => c.Packages).Returns(packages);
        //    context.Setup(c => c.Version).Returns(new GeneratorVersion());

        //    ISettings sut = new Settings(context.Object);
        //    sut.RegisterPackage("name", version);

        //    sut.GetPackageVersion("name").Should().Be(expected);
        //}
    }
}
