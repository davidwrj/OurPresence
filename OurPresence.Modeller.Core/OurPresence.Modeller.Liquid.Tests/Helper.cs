// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class Helper
    {
        public static void AssertTemplateResult(string expected, string template, object anonymousObject)
        {
            var localVariables = anonymousObject == null ? null : Hash.FromAnonymousObject(anonymousObject);
            AssertTemplateResult(expected, template, localVariables);
        }

        public static void AssertTemplateResult(string expected, string template)
        {
            AssertTemplateResult(expected: expected, template: template, anonymousObject: null);
        }

        public static void AssertTemplateResult(string expected, string template, Hash localVariables)
        {
            var parameters = new RenderParameters(System.Globalization.CultureInfo.CurrentCulture)
            {
                LocalVariables = localVariables,
            };
            var tpl = Template.Parse(template);
            var actual = tpl.Render(parameters);

            Assert.Equal(expected, actual);
        }

        [LiquidType("PropAllowed")]
        public class DataObject
        {
            public string PropAllowed { get; set; }
            public string PropDisallowed { get; set; }
        }

        public class DataObjectDrop : Drop
        {
            public DataObjectDrop(Template template)
                : base(template)
            { }

            public string Prop { get; set; }
        }
    }
}
