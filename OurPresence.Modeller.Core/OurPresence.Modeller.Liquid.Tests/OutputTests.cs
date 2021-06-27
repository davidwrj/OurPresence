// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests
{
    public class OutputTests
    {
        private static class FunnyFilter
        {
            public static string MakeFunny(string input)
            {
                return "LOL";
            }

            public static string CiteFunny(string input)
            {
                return "LOL: " + input;
            }

            public static string AddSmiley(string input, string smiley = ":-)")
            {
                return input + " " + smiley;
            }

            public static string AddTag(string input, string tag = "p", string id = "foo")
            {
                return string.Format("<{0} id=\"{1}\">{2}</{0}>", tag, id, input);
            }

            public static string Paragraph(string input)
            {
                return string.Format("<p>{0}</p>", input);
            }

            public static string LinkTo(string name, string url)
            {
                return string.Format("<a href=\"{0}\">{1}</a>", url, name);
            }
        }

        private readonly Hash _assigns;

        public  OutputTests()
        {
            _assigns = Hash.FromAnonymousObject(new
            {
                best_cars = "bmw",
                car = Hash.FromAnonymousObject(new { bmw = "good", gm = "bad" }),
                number = 3.145
            });
        }

        [Fact]
        public void TestVariable()
        {
            Assert.Equal(" bmw ", Template.Parse(" {{best_cars}} ").Render(_assigns));
        }


        string Render(CultureInfo culture)
        {

            var renderParams = new RenderParameters(culture)
                               {
                                   LocalVariables = _assigns 
                               };
            return Template.Parse("{{number}}").Render(renderParams);
        }

        [Fact]
         public void TestSeperator_Comma()
        {

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ",";
            var c = new CultureInfo("en-US")
            {
                NumberFormat = nfi
            };
            Assert.Equal("3,145", Render(c) );

        }
         [Fact]
         public void TestSeperator_Decimal()
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            var c = new CultureInfo("en-US")
            {
                NumberFormat = nfi
            };
            Assert.Equal("3.145",Render(c) );

        }

        private class ActionDisposable : IDisposable
        {
            private readonly Action _Action;

            public ActionDisposable(Action action) => _Action = action;

            public void Dispose() => _Action();
        }
        IDisposable SetCulture(CultureInfo ci)
        {
            var old = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = ci;
            return new ActionDisposable( ()=>CultureInfo.CurrentCulture = old );
        }


        [Fact]
        public void ParsingWithCommaDecimalSeparatorShouldWorkWhenPassedCultureIsDifferentToCurrentCulture()
        {
            var ci = new CultureInfo(CultureInfo.CurrentCulture.Name)
            {
                NumberFormat =
                      {
                          NumberDecimalSeparator = ","
                          , NumberGroupSeparator = "."
                      }
            };
            using (SetCulture( ci ))
            {
                var t = Template.Parse( "{{2.5}}" );
                var result = t.Render( new Hash(), CultureInfo.InvariantCulture );

                Assert.Equal( result, "2.5" );
            }
        }

        [Fact]
        public void ParsingWithInvariantCultureShouldWork()
        {
            var ci = new CultureInfo(CultureInfo.CurrentCulture.Name)
                              {
                                  NumberFormat =
                                  {
                                      NumberDecimalSeparator = ","
                                      , NumberGroupSeparator = "."
                                  }
                              };
            using (SetCulture( ci ))
            {
                float.TryParse("2.5", NumberStyles.Number, CultureInfo.InvariantCulture, out var result);

                Assert.Equal(2.5,result);
            }
        }

        [Fact]
        public void ParsingWithExplicitCultureShouldWork()
        {
            var ci = new CultureInfo( CultureInfo.CurrentCulture.Name )
                                 {
                                     NumberFormat =
                                     {
                                         NumberDecimalSeparator = ","
                                         , NumberGroupSeparator = "."
                                     }
                                 };
            using (SetCulture( ci ))
            {
                CultureInfo.CurrentCulture = ci;
                float.TryParse("2.5", NumberStyles.Number, ci, out var result);

                Assert.Equal(25, result);
            }
        }


        [Fact]
        public void ParsingWithDefaultCultureShouldWork()
        {
            var ci = new CultureInfo(CultureInfo.CurrentCulture.Name)
                              {
                                  NumberFormat =
                                  {
                                      NumberDecimalSeparator = ","
                                      , NumberGroupSeparator = "."
                                  }
                              };
            using(SetCulture( ci))
            {
                float.TryParse( "2.5", out var result);
                Assert.Equal(25,result);
            }
        }

        [Fact]
        public void TestVariableTraversing()
        {
            Assert.Equal(" good bad good ", Template.Parse(" {{car.bmw}} {{car.gm}} {{car.bmw}} ").Render(_assigns));
        }

        [Fact]
        public void TestVariablePiping()
        {
            Assert.Equal(" LOL ", Template.Parse(" {{ car.gm | make_funny }} ").Render(new RenderParameters(CultureInfo.InvariantCulture) { LocalVariables = _assigns, Filters = new[] { typeof(FunnyFilter) } }));
        }

        [Fact]
        public void TestVariablePipingWithInput()
        {
            Assert.Equal(" LOL: bad ", Template.Parse(" {{ car.gm | cite_funny }} ").Render(new RenderParameters(CultureInfo.InvariantCulture) { LocalVariables = _assigns, Filters = new[] { typeof(FunnyFilter) } }));
        }

        [Fact]
        public void TestVariablePipingWithArgs()
        {
            Assert.Equal(" bad :-( ", Template.Parse(" {{ car.gm | add_smiley : ':-(' }} ").Render(new RenderParameters(CultureInfo.InvariantCulture) { LocalVariables = _assigns, Filters = new[] { typeof(FunnyFilter) } }));
        }

        [Fact]
        public void TestVariablePipingWithNoArgs()
        {
            Assert.Equal(" bad :-) ", Template.Parse(" {{ car.gm | add_smiley }} ").Render(new RenderParameters(CultureInfo.InvariantCulture) { LocalVariables = _assigns, Filters = new[] { typeof(FunnyFilter) } }));
        }

        [Fact]
        public void TestMultipleVariablePipingWithArgs()
        {
            Assert.Equal(" bad :-( :-( ", Template.Parse(" {{ car.gm | add_smiley : ':-(' | add_smiley : ':-(' }} ").Render(new RenderParameters(CultureInfo.InvariantCulture) { LocalVariables = _assigns, Filters = new[] { typeof(FunnyFilter) } }));
        }

        [Fact]
        public void TestVariablePipingWithArgs2()
        {
            Assert.Equal(" <span id=\"bar\">bad</span> ", Template.Parse(" {{ car.gm | add_tag : 'span', 'bar' }} ").Render(new RenderParameters(CultureInfo.InvariantCulture) { LocalVariables = _assigns, Filters = new[] { typeof(FunnyFilter) } }));
        }

        [Fact]
        public void TestVariablePipingWithWithVariableArgs()
        {
            Assert.Equal(" <span id=\"good\">bad</span> ", Template.Parse(" {{ car.gm | add_tag : 'span', car.bmw }} ").Render(new RenderParameters(CultureInfo.InvariantCulture) { LocalVariables = _assigns, Filters = new[] { typeof(FunnyFilter) } }));
        }

        [Fact]
        public void TestMultiplePipings()
        {
            Assert.Equal(" <p>LOL: bmw</p> ", Template.Parse(" {{ best_cars | cite_funny | paragraph }} ").Render(new RenderParameters(CultureInfo.InvariantCulture) { LocalVariables = _assigns, Filters = new[] { typeof(FunnyFilter) } }));
        }

        [Fact]
        public void TestLinkTo()
        {
            Assert.Equal(" <a href=\"http://typo.leetsoft.com\">Typo</a> ", Template.Parse(" {{ 'Typo' | link_to: 'http://typo.leetsoft.com' }} ").Render(new RenderParameters(CultureInfo.InvariantCulture) { LocalVariables = _assigns, Filters = new[] { typeof(FunnyFilter) } }));
        }
    }
}
