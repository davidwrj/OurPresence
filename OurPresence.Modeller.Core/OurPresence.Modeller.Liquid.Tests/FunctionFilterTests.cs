using FluentAssertions;
using System.Globalization;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class FunctionFilterTests
    {
        private Context _context = new Context(CultureInfo.InvariantCulture);

        [Fact]
        public void AddingFunctions()
        {
            _context["var"] = 2;
            _context.AddFilter<int, string>("AddTwo", i => (i + 2).ToString(CultureInfo.InvariantCulture));
            new Variable("var | add_two").Render(_context).Should().Be("4");
        }

        [Fact]
        public void AddingAnonimousFunctionWithClosure()
        {
            _context["var"] = 2;
            int x = 2;

            // (x=(i + x)) is to forbid JITC to inline x and force it to create non-static closure

            _context.AddFilter<int, string>("AddTwo", i => (x=(i + x)).ToString(CultureInfo.InvariantCulture));
            new Variable("var | add_two").Render(_context).Should().Be("4");

            //this is done, to forbid JITC to inline x 
            x.Should().Be(4);
        }

        [Fact]
        public void AddingMethodInfo()
        {
            _context["var"] = 2;
            _context.AddFilter<int, string>("AddTwo", i => (i + 2).ToString(CultureInfo.InvariantCulture));
            new Variable("var | add_two").Render(_context).Should().Be("4");
        }
    }
}