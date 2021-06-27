// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using OurPresence.Modeller.Liquid.FileSystems;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests.Tags
{
    public class InheritanceTests
    {
        private class TestFileSystem : IFileSystem
        {
            public string ReadTemplateFile(Context context, string templateName)
            {
                string templatePath = (string)context[templateName];

                switch(templatePath)
                {
                    case "simple":
                        return "test";
                    case "complex":
                        return @"some markup here...
                             {% block thing %}
                                 thing block
                             {% endblock %}
                             {% block another %}
                                 another block
                             {% endblock %}
                             ...and some markup here";
                    case "nested":
                        return @"{% extends 'complex' %}
                             {% block thing %}
                                another thing (from nested)
                             {% endblock %}";
                    case "outer":
                        return "{% block start %}{% endblock %}A{% block outer %}{% endblock %}Z";
                    case "middle":
                        return @"{% extends 'outer' %}
                             {% block outer %}B{% block middle %}{% endblock %}Y{% endblock %}";
                    case "middleunless":
                        return @"{% extends 'outer' %}
                             {% block outer %}B{% unless nomiddle %}{% block middle %}{% endblock %}{% endunless %}Y{% endblock %}";
                    default:
                        return @"{% extends 'complex' %}
                             {% block thing %}
                                thing block (from nested)
                             {% endblock %}";
                }
            }
        }

        [Fact]
        public void CanOutputTheContentsOfTheExtendedTemplate()
        {
            Template template = Template.Parse(
                                    @"{% extends 'simple' %}
                    {% block thing %}
                        yeah
                    {% endblock %}");

            template.Render().Should().Contain("test");
        }

        [Fact]
        public void CanInherit()
        {
            Template template = Template.Parse(@"{% extends 'complex' %}");

            template.Render().Should().Contain("thing block");
        }

        [Fact]
        public void CanInheritAndReplaceBlocks()
        {
            Template template = Template.Parse(
                                    @"{% extends 'complex' %}
                    {% block another %}
                      new content for another
                    {% endblock %}");

            template.Render().Should().Contain("new content for another");
        }

        [Fact]
        public void CanProcessNestedInheritance()
        {
            Template template = Template.Parse(
                                    @"{% extends 'nested' %}
                  {% block thing %}
                  replacing block thing
                  {% endblock %}");

            template.Render().Should().Contain("replacing block thing");
            template.Render().Should().NotContain("thing block");
        }

        [Fact]
        public void CanRenderSuper()
        {
            Template template = Template.Parse(
                                    @"{% extends 'complex' %}
                    {% block another %}
                        {{ block.super }} + some other content
                    {% endblock %}");

            template.Render().Should().Contain("another block");
            template.Render().Should().Contain("some other content");
        }

        [Fact]
        public void CanDefineBlockInInheritedBlock()
        {
            Template template = Template.Parse(
                                    @"{% extends 'middle' %}
                  {% block middle %}C{% endblock %}");
            Assert.Equal("ABCYZ", template.Render());
        }

        [Fact]
        public void CanDefineContentInInheritedBlockFromAboveParent()
        {
            Template template = Template.Parse(@"{% extends 'middle' %}
                  {% block start %}!{% endblock %}");
            Assert.Equal("!ABYZ", template.Render());
        }

        [Fact]
        public void CanRenderBlockContainedInConditional()
        {
            Template template = Template.Parse(
                                    @"{% extends 'middleunless' %}
                  {% block middle %}C{% endblock %}");
            Assert.Equal("ABCYZ", template.Render());

            template = Template.Parse(
                @"{% extends 'middleunless' %}
                  {% block start %}{% assign nomiddle = true %}{% endblock %}
                  {% block middle %}C{% endblock %}");
            Assert.Equal("ABYZ", template.Render());
        }

        [Fact]
        public void RepeatedRendersProduceSameResult()
        {
            Template template = Template.Parse(
                                    @"{% extends 'middle' %}
                  {% block start %}!{% endblock %}
                  {% block middle %}C{% endblock %}");
            Assert.Equal("!ABCYZ", template.Render());
            Assert.Equal("!ABCYZ", template.Render());
        }

        [Fact]
        public void TestExtendFromTemplateFileSystem()
        {
            var fileSystem = new IncludeTagTests.TestTemplateFileSystem(new TestFileSystem());
            for(int i = 0; i < 2; ++i)
            {
                Template template = Template.Parse(
                                    @"{% extends 'simple' %}
                    {% block thing %}
                        yeah
                    {% endblock %}", fileSystem);
                template.Render().Should().Contain("test");
            }
            fileSystem.CacheHitTimes.Should().Be(1);
        }
    }
}
