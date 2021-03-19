using OurPresence.Modeller.Liquid.NamingConventions;
using Xunit;
using System;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class Helper
    {
        public static void LockTemplateStaticVars(INamingConvention namingConvention, Action test)
        {
            //Have to lock Template.NamingConvention for this test to
            //prevent other tests from being run simultaneously that
            //require the default naming convention.
            var currentNamingConvention = Template.NamingConvention;
            var currentSyntax = Template.DefaultSyntaxCompatibilityLevel;
            var currentIsRubyDateFormat = Liquid.UseRubyDateFormat;
            lock (Template.NamingConvention)
            {
                Template.NamingConvention = namingConvention;

                try
                {
                    test();
                }
                finally
                {
                    Template.NamingConvention = currentNamingConvention;
                    Template.DefaultSyntaxCompatibilityLevel = currentSyntax;
                    Liquid.UseRubyDateFormat = currentIsRubyDateFormat;
                }
            }
        }

        public static void AssertTemplateResult(string expected, string template, object anonymousObject, INamingConvention namingConvention, SyntaxCompatibility syntax = SyntaxCompatibility.Liquid20)
        {
            LockTemplateStaticVars(namingConvention, () =>
            {
                var localVariables = anonymousObject == null ? null : Hash.FromAnonymousObject(anonymousObject);
                AssertTemplateResult(expected, template, localVariables, syntax);
            });
        }

        public static void AssertTemplateResult(string expected, string template, INamingConvention namingConvention)
        {
            AssertTemplateResult(expected: expected, template: template, anonymousObject: null, namingConvention: namingConvention);
        }

        public static void AssertTemplateResult(string expected, string template, SyntaxCompatibility syntax = SyntaxCompatibility.Liquid20)
        {
            AssertTemplateResult(expected: expected, template: template, localVariables: null, syntax: syntax);
        }

        public static void AssertTemplateResult(string expected, string template, Hash localVariables, SyntaxCompatibility syntax = SyntaxCompatibility.Liquid20)
        {
            var parameters = new RenderParameters(System.Globalization.CultureInfo.CurrentCulture)
            {
                LocalVariables = localVariables,
                SyntaxCompatibilityLevel = syntax
            };
            Assert.Equal(expected, Template.Parse(template).Render(parameters));
        }

        [LiquidType("PropAllowed")]
        public class DataObject
        {
            public string PropAllowed { get; set; }
            public string PropDisallowed { get; set; }
        }

        public class DataObjectDrop : Drop
        {
            public string Prop { get; set; }
        }
    }
}
