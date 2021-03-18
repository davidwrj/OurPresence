using System;
using FluentAssertions;
using OurPresence.Modeller.Liquid.Tags;
using OurPresence.Modeller.Liquid.Tests.Framework;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class BlockTests
    {
        [Fact]
        public void TestBlankspace()
        {
            Template template = Template.Parse("  ");
            template.Root.NodeList.Should().BeEquivalentTo(new[] { "  " });
        }

        [Fact]
        public void TestVariableBeginning()
        {
            Template template = Template.Parse("{{funk}}  ");
            Assert.Equal(2, template.Root.NodeList.Count);
            ExtendedCollectionAssert.AllItemsAreInstancesOfTypes(template.Root.NodeList,
                new[] { typeof(Variable), typeof(string) });
        }

        [Fact]
        public void TestVariableEnd()
        {
            Template template = Template.Parse("  {{funk}}");
            Assert.Equal(2, template.Root.NodeList.Count);
            ExtendedCollectionAssert.AllItemsAreInstancesOfTypes(template.Root.NodeList,
                new[] { typeof(string), typeof(Variable) });
        }

        [Fact]
        public void TestVariableMiddle()
        {
            Template template = Template.Parse("  {{funk}}  ");
            Assert.Equal(3, template.Root.NodeList.Count);
            ExtendedCollectionAssert.AllItemsAreInstancesOfTypes(template.Root.NodeList,
                new[] { typeof(string), typeof(Variable), typeof(string) });
        }

        [Fact]
        public void TestVariableManyEmbeddedFragments()
        {
            Template template = Template.Parse("  {{funk}} {{so}} {{brother}} ");
            Assert.Equal(7, template.Root.NodeList.Count);
            ExtendedCollectionAssert.AllItemsAreInstancesOfTypes(template.Root.NodeList,
                new[]
                {
                    typeof(string), typeof(Variable), typeof(string),
                    typeof(Variable), typeof(string), typeof(Variable),
                    typeof(string)
                });
        }

        [Fact]
        public void TestWithBlock()
        {
            Template template = Template.Parse("  {% comment %} {% endcomment %} ");
            Assert.Equal(3, template.Root.NodeList.Count);
            ExtendedCollectionAssert.AllItemsAreInstancesOfTypes(template.Root.NodeList,
                new[] { typeof(string), typeof(Comment), typeof(string) });
        }

        [Fact]
        public void TestWithCustomTag()
        {
            Template.RegisterTag<Block>("testtag");
            Action action = () => Template.Parse("{% testtag %} {% endtesttag %}");
            action.Should().NotThrow();
            //Assert.DoesNotThrow(() => Template.Parse("{% testtag %} {% endtesttag %}"));
        }

        [Fact]
        public void TestWithCustomTagFactory()
        {
            Template.RegisterTagFactory(new CustomTagFactory());
            Template result = null;
            Action action = () => result = Template.Parse("{% custom %}");
            action.Should().NotThrow();

            //Assert.DoesNotThrow(() => result = Template.Parse("{% custom %}"));
            Assert.Equal("I am a custom tag" + Environment.NewLine, result.Render());
        }

        public class CustomTagFactory : ITagFactory
        {
            public string TagName
            {
                get { return "custom"; }
            }

            public Tag Create()
            {
                return new CustomTag();
            }

            public class CustomTag : Tag
            {
                public override void Render(Context context, System.IO.TextWriter result)
                {
                    result.WriteLine("I am a custom tag");
                }
            }
        }

    }
}
