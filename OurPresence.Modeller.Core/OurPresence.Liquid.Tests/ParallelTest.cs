using NUnit.Framework;
using System.Threading.Tasks;

namespace OurPresence.Liquid.Tests {
    [TestFixture]
    public class ParallelTest {
        [Test]
        public void TestCachedTemplateRender() {
            Template template = Template.Parse(@"{% assign foo = 'from instance assigns' %}{{foo}}");
            template.MakeThreadSafe();

            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 30 };

            Parallel.For(0, 10000, parallelOptions, (x) => Assert.AreEqual("from instance assigns", template.Render()));
        }
    }
}
