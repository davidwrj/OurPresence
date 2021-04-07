using OurPresence.Modeller.Liquid.NamingConventions;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class NamingConventionTests
    {
        [Fact]
        public void TestRubySimpleName()
        {
            RubyNamingConvention namingConvention = new RubyNamingConvention();
            Assert.Equal("test", namingConvention.GetMemberName("Test"));
        }

        [Fact]
        public void TestRubyComplexName()
        {
            RubyNamingConvention namingConvention = new RubyNamingConvention();
            Assert.Equal("hello_world", namingConvention.GetMemberName("HelloWorld"));
        }

        [Fact]
        public void TestRubyMoreComplexName()
        {
            RubyNamingConvention namingConvention = new RubyNamingConvention();
            Assert.Equal("hello_cruel_world", namingConvention.GetMemberName("HelloCruelWorld"));
        }

        [Fact]
        public void TestRubyFullUpperCase()
        {
            RubyNamingConvention namingConvention = new RubyNamingConvention();
            Assert.Equal("id", namingConvention.GetMemberName("ID"));
            Assert.Equal("hellocruelworld", namingConvention.GetMemberName("HELLOCRUELWORLD"));
        }

        [Fact]
        public void TestRubyWithTurkishCulture()
        {
            using (CultureHelper.SetCulture("tr-TR"))
            {
                RubyNamingConvention namingConvention = new RubyNamingConvention();

                // in Turkish ID.ToLower() returns a localized i, and this fails
                Assert.Equal("id", namingConvention.GetMemberName("ID"));
            }
        }

        [Fact]
        public void TestCSharpConventionDoesNothing()
        {
            CSharpNamingConvention namingConvention = new CSharpNamingConvention();
            Assert.Equal("Test", namingConvention.GetMemberName("Test"));
        }
    }
}
