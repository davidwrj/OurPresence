using NUnit.Framework;

namespace OurPresence.Liquid.Tests.Tags
{
    [TestFixture]
    public class RawTests
    {
        [Test]
        public void TestTagInRaw ()
        {
            Helper.AssertTemplateResult ("{% comment %} test {% endcomment %}",
                "{% raw %}{% comment %} test {% endcomment %}{% endraw %}");
        }

        [Test]
        public void TestOutputInRaw ()
        {
            Helper.AssertTemplateResult ("{{ test }}",
                "{% raw %}{{ test }}{% endraw %}");
        }
    }
}
