using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Xunit;
using NSubstitute;

namespace OurPresence.Modeller.CoreTests
{
    public class PackageServiceFacts
    {
        private IEnumerable<IPackage> GetDefaultTargetTestPackages() => new List<IPackage> { new Package("Package1", "1.2.3"), new Package("Package2", "2.3") };

        [Fact]
        public void PackageService_LoadsFiles()
        {
            var packages = GetDefaultTargetTestPackages();

            var logger = Substitute.For<ILogger<IPackageService>>();
            var context = Substitute.For<IContext>();
            context.Settings.Target.Returns("net5.0");
            context.Settings.LocalFolder.Returns(System.IO.Path.GetTempPath());

            var loader = Substitute.For<ILoader<IEnumerable<IPackage>>>();
            loader.TryLoad(Arg.Any<string>(), out Arg.Any<IEnumerable<IPackage>>())
                .Returns(x => 
                {
                    x[1] = packages;
                    return true;
                });
            IPackageService sut = new PackageService(loader, logger);
            sut.Refresh("");
            sut.Items.Should().BeEquivalentTo(packages);
        }

        [Fact]
        public void PackageService_ThrowsMissingTargetException_WhenFolderNotValid()
        {
            var packages = GetDefaultTargetTestPackages();

            var logger = Substitute.For<ILogger<IPackageService>>();
            var context = Substitute.For<IContext>();
            context.Settings.Target.Returns("net5.0");
            context.Settings.LocalFolder.Returns(System.IO.Path.GetTempPath());

            var loader = Substitute.For<ILoader<IEnumerable<IPackage>>>();
            loader.TryLoad(Arg.Any<string>(), out Arg.Any<IEnumerable<IPackage>>())
                .Returns(x =>
                {
                    x[1] = packages;
                    return false;
                });

            IPackageService sut = new PackageService(loader, logger);
            sut.Refresh("");

            sut.Items.Should().HaveCount(0);
        }
    }
}
