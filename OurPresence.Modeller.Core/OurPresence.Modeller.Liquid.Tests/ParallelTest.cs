using Xunit;
using System.Threading.Tasks;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class ParallelTest {
        [Fact]
        public void TestCachedTemplateRender() {
            Template template = Template.Parse(@"{% assign foo = 'from instance assigns' %}{{foo}}");
            template.MakeThreadSafe();

            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 30 };

            Parallel.For(0, 10000, parallelOptions, (x) => Assert.Equal("from instance assigns", template.Render()));
        }
    }
}
