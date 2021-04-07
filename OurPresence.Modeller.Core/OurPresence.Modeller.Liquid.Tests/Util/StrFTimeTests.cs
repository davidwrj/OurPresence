using System;
using System.Globalization;
using FluentAssertions;
using OurPresence.Modeller.Liquid.Util;
using Xunit;

namespace OurPresence.Modeller.Liquid.Tests.Util
{
    // See https://help.shopify.com/themes/liquid/filters/additional-filters#date
    public class StrFTimeTests
    {
        [InlineData("%a", "Sun")]
        [InlineData("%A", "Sunday")]
        [InlineData("%b", "Jan")]
        [InlineData("%B", "January")]
        [InlineData("%c", "Sun Jan 08 14:32:14 2012")]
        [InlineData("%C", "20")]
        [InlineData("%d", "08")]
        [InlineData("%e", " 8")]
        [InlineData("%h", "Jan")]
        [InlineData("%H", "14")]
        [InlineData("%I", "02")]
        [InlineData("%j", "008")]
        [InlineData("%k", "14")]
        [InlineData("%l", "2")]
        [InlineData("%m", "01")]
        [InlineData("%M", "32")]
        [InlineData("%P", "pm")]
        [InlineData("%p", "PM")]
        [InlineData("%S", "14")]
        [InlineData("%u", "7")]
        [InlineData("%U", "02")]
        [InlineData("%W", "01")]
        [InlineData("%w", "0")]
        [InlineData("%x", "08/01/2012")]
        [InlineData("%X", "14:32:14")]
        [InlineData("%y", "12")]
        [InlineData("%Y", "2012")]
        [InlineData("%", "%")]
        public void TestFormat(string format, string expected)
        {
            using (CultureHelper.SetCulture("en-GB")) 
            {
                CultureInfo.CurrentCulture.Should().Be(new CultureInfo("en-GB"));
                // Assert.That(CultureInfo.CurrentCulture, Is.EqualTo(new CultureInfo("en-GB")));

                var date = new DateTime(2012, 1, 8, 14, 32, 14);
                var localResult = date.ToStrFTime(format);
                var utcResult = new DateTimeOffset(date, TimeSpan.FromHours(0)).ToStrFTime(format);
                Assert.Equal(localResult, utcResult);
                var estResult = new DateTimeOffset(date, TimeSpan.FromHours(-5)).ToStrFTime(format);
                Assert.Equal(utcResult, estResult);
                localResult.Should().Be(expected);
            }
        }

        [Fact]
        public void TestEpoch()
        {
            using (CultureHelper.SetCulture("en-GB"))
            {
                CultureInfo.CurrentCulture.Should().Be(new CultureInfo("en-GB"));
                var date = new DateTime(2012, 1, 8, 14, 32, 14);
                var localResult = date.ToStrFTime("%s");
                Assert.Equal("1326033134", localResult);
                var utcResult = new DateTimeOffset(date, TimeSpan.FromHours(0)).ToStrFTime("%s");
                Assert.Equal("1326033134", utcResult);
                var estResult = new DateTimeOffset(date, TimeSpan.FromHours(-5)).ToStrFTime("%s");
                Assert.Equal("1326051134", estResult);
            }
        }

        [Fact]
        public void TestTimeZone()
        {
            var now = DateTimeOffset.Now;
            string timeZoneOffset = now.ToString("zzz");
            now.DateTime.ToStrFTime("%Z").Should().Be(timeZoneOffset);
        }
    }
}
