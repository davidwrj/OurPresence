using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using OurPresence.Modeller.Liquid.Exceptions;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class ExceptionHandlingTests
    {
        private class ExceptionDrop : Drop
        {
            public void ArgumentException()
            {
                throw new Exceptions.ArgumentException("argument exception");
            }

            public void SyntaxException()
            {
                throw new SyntaxException("syntax exception");
            }

            public void InterruptException()
            {
                throw new InterruptException("interrupted");
            }
        }

        [Fact]
        public void TestSyntaxException()
        {
            Template template = null;
            Action action = () => { template = Template.Parse(" {{ errors.syntax_exception }} "); };
            action.Should().NotThrow();
            string result = template.Render(Hash.FromAnonymousObject(new { errors = new ExceptionDrop() }));
            Assert.Equal(" Liquid syntax error: syntax exception ", result);

            template.Errors.Should().HaveCount(1);
            template.Errors[0].Should().BeOfType<SyntaxException>();
        }

        [Fact]
        public void TestArgumentException()
        {
            Template template = null;
            Action action = () => { template = Template.Parse(" {{ errors.argument_exception }} "); };
            action.Should().NotThrow();
            string result = template.Render(Hash.FromAnonymousObject(new { errors = new ExceptionDrop() }));
            Assert.Equal(" Liquid error: argument exception ", result);

            template.Errors.Should().HaveCount(1);
            template.Errors[0].Should().BeOfType<Exceptions.ArgumentException>();
        }

        [Fact]
        public void TestMissingEndTagParseTimeError()
        {
            Assert.Throws<SyntaxException>(() => Template.Parse(" {% for a in b %} ... "));
        }

        [Fact]
        public void TestUnrecognizedOperator()
        {
            Template template = null;
            Action action = () => { template = Template.Parse(" {% if 1 =! 2 %}ok{% endif %} "); };
            action.Should().NotThrow();
            Assert.Equal(" Liquid error: Unknown operator =! ", template.Render());

            template.Errors.Should().HaveCount(1);
            template.Errors[0].Should().BeOfType<Exceptions.ArgumentException>();
        }

        [Fact]
        public void TestInterruptException()
        {
            Template template = null;
            Action action = () => { template = Template.Parse(" {{ errors.interrupt_exception }} "); };
            action.Should().NotThrow();
            var localVariables = Hash.FromAnonymousObject(new { errors = new ExceptionDrop() });
            var exception = Assert.Throws<InterruptException>(() => template.Render(localVariables));

            Assert.Equal("interrupted", exception.Message);
        }

        [Fact]
        public void TestMaximumIterationsExceededError()
        {
            var template = Template.Parse(" {% for i in (1..100000) %} {{ i }} {% endfor %} ");
            Assert.Throws<MaximumIterationsExceededException>(() =>
            {
                template.Render(new RenderParameters(CultureInfo.InvariantCulture)
                {
                    MaxIterations = 50
                });
            });
        }

        [Fact]
        public void TestTimeoutError()
        {
            var template = Template.Parse(" {% for i in (1..1000000) %} {{ i }} {% endfor %} ");
            Assert.Throws<System.TimeoutException>(() =>
            {
                template.Render(new RenderParameters(CultureInfo.InvariantCulture)
                {
                    Timeout = 100 //ms
                });
            });
        }

        [Fact]
        public void TestOperationCancelledError()
        {
            var template = Template.Parse(" {% for i in (1..1000000) %} {{ i }} {% endfor %} ");
            var source = new CancellationTokenSource(100);
            var context = new Context(
                environments: new List<Hash>(),
                outerScope: new Hash(),
                registers: new Hash(),
                errorsOutputMode: ErrorsOutputMode.Rethrow,
                maxIterations: 0,
                formatProvider: CultureInfo.InvariantCulture,
                cancellationToken: source.Token);

            Assert.Throws<System.OperationCanceledException>(() =>
            {
                template.Render(RenderParameters.FromContext(context, CultureInfo.InvariantCulture));
            });
        }

        [Fact]
        public void TestErrorsOutputModeRethrow()
        {
            var template = Template.Parse("{{test}}");
            Hash assigns = new Hash((h, k) => { throw new SyntaxException("Unknown variable '" + k + "'"); });

            Assert.Throws<SyntaxException>(() =>
            {
                var output = template.Render(new RenderParameters(CultureInfo.InvariantCulture)
                {
                    LocalVariables = assigns,
                    ErrorsOutputMode = ErrorsOutputMode.Rethrow
                });
            });
        }

        [Fact]
        public void TestErrorsOutputModeSuppress()
        {
            var template = Template.Parse("{{test}}");
            Hash assigns = new Hash((h, k) => { throw new SyntaxException("Unknown variable '" + k + "'"); });

            var output = template.Render(new RenderParameters(CultureInfo.InvariantCulture)
            {
                LocalVariables = assigns,
                ErrorsOutputMode = ErrorsOutputMode.Suppress
            });
            Assert.Equal("", output);
        }

        [Fact]
        public void TestErrorsOutputModeDisplay()
        {
            var template = Template.Parse("{{test}}");
            Hash assigns = new Hash((h, k) => { throw new SyntaxException("Unknown variable '" + k + "'"); });

            var output = template.Render(new RenderParameters(CultureInfo.InvariantCulture)
            {
                LocalVariables = assigns,
                ErrorsOutputMode = ErrorsOutputMode.Display
            });
            Assert.NotEmpty(output);
        }
    }
}
