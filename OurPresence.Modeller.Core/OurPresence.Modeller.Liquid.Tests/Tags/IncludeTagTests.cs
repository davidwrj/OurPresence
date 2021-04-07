using System;
using OurPresence.Modeller.Liquid.Exceptions;
using OurPresence.Modeller.Liquid.FileSystems;
using Xunit;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;

namespace OurPresence.Modeller.Liquid.Tests.Tags
{
    public class IncludeTagTests
    {
        private class TestFileSystem : IFileSystem
        {
            public string ReadTemplateFile(Context context, string templateName)
            {
                string templatePath = (string) context[templateName];

                switch (templatePath)
                {
                    case "product":
                        return "Product: {{ product.title }} ";

                    case "locale_variables":
                        return "Locale: {{echo1}} {{echo2}}";

                    case "variant":
                        return "Variant: {{ variant.title }}";

                    case "nested_template":
                        return "{% include 'header' %} {% include 'body' %} {% include 'footer' %}";

                    case "body":
                        return "body {% include 'body_detail' %}";

                    case "nested_product_template":
                        return "Product: {{ nested_product_template.title }} {%include 'details'%} ";

                    case "recursively_nested_template":
                        return "-{% include 'recursively_nested_template' %}";

                    case "pick_a_source":
                        return "from TestFileSystem";

                    default:
                        return templatePath;
                }
            }
        }

        internal class TestTemplateFileSystem : ITemplateFileSystem
        {
            private IDictionary<string, Template> _templateCache = new Dictionary<string, Template>();
            private IFileSystem _baseFileSystem = null;
            private int _cacheHitTimes;
            public int CacheHitTimes { get { return _cacheHitTimes; } }

            public TestTemplateFileSystem(IFileSystem baseFileSystem)
            {
                _baseFileSystem = baseFileSystem;
            }

            public string ReadTemplateFile(Context context, string templateName)
            {
                return _baseFileSystem.ReadTemplateFile(context, templateName);
            }

            public Template GetTemplate(Context context, string templateName)
            {
                Template template;
                if (_templateCache.TryGetValue(templateName, out template))
                {
                    ++_cacheHitTimes;
                    return template;
                }
                var result = ReadTemplateFile(context, templateName);
                template = Template.Parse(result);
                _templateCache[templateName] = template;
                return template;
            }
        }

        private class OtherFileSystem : IFileSystem
        {
            public string ReadTemplateFile(Context context, string templateName)
            {
                return "from OtherFileSystem";
            }
        }

        private class InfiniteFileSystem : IFileSystem
        {
            public string ReadTemplateFile(Context context, string templateName)
            {
                return "-{% include 'loop' %}";
            }
        }

        public IncludeTagTests()
        {
            Template.FileSystem = new TestFileSystem();
        }

        [Fact]
        public void TestIncludeTagMustNotBeConsideredError()
        {
            Assert.Empty(Template.Parse("{% include 'product_template' %}").Errors);
            Action action = () => Template.Parse("{% include 'product_template' %}").Render(new RenderParameters(CultureInfo.InvariantCulture) { ErrorsOutputMode= ErrorsOutputMode.Rethrow });
            action.Should().NotThrow();
        }

        [Fact]
        public void TestIncludeTagLooksForFileSystemInRegistersFirst()
        {
            Assert.Equal("from OtherFileSystem", Template.Parse("{% include 'pick_a_source' %}").Render(new RenderParameters(CultureInfo.InvariantCulture) { Registers = Hash.FromAnonymousObject(new { file_system = new OtherFileSystem() }) }));
        }

        [Fact]
        public void TestIncludeTagWith()
        {
            Assert.Equal("Product: Draft 151cm ", Template.Parse("{% include 'product' with products[0] %}").Render(Hash.FromAnonymousObject(new { products = new[] { Hash.FromAnonymousObject(new { title = "Draft 151cm" }), Hash.FromAnonymousObject(new { title = "Element 155cm" }) } })));
        }

        [Fact]
        public void TestIncludeTagWithDefaultName()
        {
            Assert.Equal("Product: Draft 151cm ", Template.Parse("{% include 'product' %}").Render(Hash.FromAnonymousObject(new { product = Hash.FromAnonymousObject(new { title = "Draft 151cm" }) })));
        }

        [Fact]
        public void TestIncludeTagFor()
        {
            Assert.Equal("Product: Draft 151cm Product: Element 155cm ", Template.Parse("{% include 'product' for products %}").Render(Hash.FromAnonymousObject(new { products = new[] { Hash.FromAnonymousObject(new { title = "Draft 151cm" }), Hash.FromAnonymousObject(new { title = "Element 155cm" }) } })));
        }

        [Fact]
        public void TestIncludeTagWithLocalVariables()
        {
            Assert.Equal("Locale: test123 ", Template.Parse("{% include 'locale_variables' echo1: 'test123' %}").Render());
        }

        [Fact]
        public void TestIncludeTagWithMultipleLocalVariables()
        {
            Assert.Equal("Locale: test123 test321", Template.Parse("{% include 'locale_variables' echo1: 'test123', echo2: 'test321' %}").Render());
        }

        [Fact]
        public void TestIncludeTagWithMultipleLocalVariablesFromContext()
        {
            Assert.Equal("Locale: test123 test321",
                Template.Parse("{% include 'locale_variables' echo1: echo1, echo2: more_echos.echo2 %}").Render(Hash.FromAnonymousObject(new { echo1 = "test123", more_echos = Hash.FromAnonymousObject(new { echo2 = "test321" }) })));
        }

        [Fact]
        public void TestNestedIncludeTag()
        {
            Assert.Equal("body body_detail", Template.Parse("{% include 'body' %}").Render());

            Assert.Equal("header body body_detail footer", Template.Parse("{% include 'nested_template' %}").Render());
        }

        [Fact]
        public void TestNestedIncludeTagWithVariable()
        {
            Assert.Equal("Product: Draft 151cm details ",
                Template.Parse("{% include 'nested_product_template' with product %}").Render(Hash.FromAnonymousObject(new { product = Hash.FromAnonymousObject(new { title = "Draft 151cm" }) })));

            Assert.Equal("Product: Draft 151cm details Product: Element 155cm details ",
                Template.Parse("{% include 'nested_product_template' for products %}").Render(Hash.FromAnonymousObject(new { products = new[] { Hash.FromAnonymousObject(new { title = "Draft 151cm" }), Hash.FromAnonymousObject(new { title = "Element 155cm" }) } })));
        }

        [Fact]
        public void TestRecursivelyIncludedTemplateDoesNotProductEndlessLoop()
        {
            Template.FileSystem = new InfiniteFileSystem();

            Assert.Throws<StackLevelException>(() => Template.Parse("{% include 'loop' %}").Render(new RenderParameters(CultureInfo.InvariantCulture) { ErrorsOutputMode = ErrorsOutputMode.Rethrow }));
        }

        [Fact]
        public void TestDynamicallyChosenTemplate()
        {
            Assert.Equal("Test123", Template.Parse("{% include template %}").Render(Hash.FromAnonymousObject(new { template = "Test123" })));
            Assert.Equal("Test321", Template.Parse("{% include template %}").Render(Hash.FromAnonymousObject(new { template = "Test321" })));

            Assert.Equal("Product: Draft 151cm ", Template.Parse("{% include template for product %}").Render(Hash.FromAnonymousObject(new { template = "product", product = Hash.FromAnonymousObject(new { title = "Draft 151cm" }) })));
        }

        [Fact]
        public void TestUndefinedTemplateVariableWithTestFileSystem()
        {
            Assert.Equal(" hello  world ", Template.Parse(" hello {% include notthere %} world ").Render());
        }

        [Fact]
        public void TestUndefinedTemplateVariableWithLocalFileSystem()
        {
            Template.FileSystem = new LocalFileSystem(string.Empty);
            Assert.Throws<FileSystemException>(() => Template.Parse(" hello {% include notthere %} world ").Render(new RenderParameters(CultureInfo.InvariantCulture)
            {
                ErrorsOutputMode = ErrorsOutputMode.Rethrow
            }));
        }

        [Fact]
        public void TestMissingTemplateWithLocalFileSystem()
        {
            Template.FileSystem = new LocalFileSystem(string.Empty);
            Assert.Throws<FileSystemException>(() => Template.Parse(" hello {% include 'doesnotexist' %} world ").Render(new RenderParameters(CultureInfo.InvariantCulture)
            {
                ErrorsOutputMode = ErrorsOutputMode.Rethrow
            }));
        }

        [Fact]
        public void TestIncludeFromTemplateFileSystem()
        {
            var fileSystem = new TestTemplateFileSystem(new TestFileSystem());
            Template.FileSystem = fileSystem;
            for (int i = 0; i < 2; ++i)
            {
                Assert.Equal("Product: Draft 151cm ", Template.Parse("{% include 'product' with products[0] %}").Render(Hash.FromAnonymousObject(new { products = new[] { Hash.FromAnonymousObject(new { title = "Draft 151cm" }), Hash.FromAnonymousObject(new { title = "Element 155cm" }) } })));
            }
            Assert.Equal(fileSystem.CacheHitTimes, 1);
        }
    }
}
