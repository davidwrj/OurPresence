using OurPresence.Modeller.Generator;
using FluentAssertions;
using Xunit;

namespace OurPresence.Modeller.CoreTests
{
    public class GeneratorConfigurationFacts
    {
        [Fact]
        public void GeneratorConfiguration_Packages_DontReturnNull()
        {
            var sut = new GeneratorConfiguration();
            sut.Packages.Should().NotBeNull();
        }
    }
}
