using System;
using System.Globalization;
using System.Threading;
using OurPresence.Core.Money.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace OurPresence.Core.Money.Tests.AmountParsableSpec
{
    public class GivenIWantToParseImplicitCurrency
    {
        public GivenIWantToParseImplicitCurrency()
        {
            _ = Currency.GetAllCurrencies();
        }

        [Fact, UseCulture("nl-BE")]
        public void WhenInBelgiumDutchSpeaking_ThenThisShouldSucceed()
        {
            var euro = Amount.Parse("€ 765,43");

            euro.Currency.Code.Should().Be("EUR");

            euro.Should().Be(new Amount(765.43m, "EUR"));
        }

        [Fact, UseCulture("fr-BE")]
        public void WhenInBelgiumFrenchSpeaking_ThenThisShouldSucceed()
        {
            var euro = Amount.Parse("765,43 €");

            euro.Currency.Code.Should().Be("EUR");

            euro.Should().Be(new Amount(765.43, "EUR"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingNumberWithoutCurrency_ThenThisUseCurrencyOfCurrentCulture()
        {
            var euro = Amount.Parse("765,43");

            euro.Currency.Code.Should().Be("EUR");

            euro.Should().Be(new Amount(765.43, "EUR"));
        }

        [Fact, UseCulture("ja-JP")]
        public void WhenParsingYenYuanSymbolInJapan_ThenThisShouldReturnJapaneseYen()
        {
            var currency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            var yen = Amount.Parse("¥ 765",currency);

            yen.Currency.Code.Should().Be("JPY");

            yen.Should().Be(new Amount(765m, "JPY"));
        }

        [Fact, UseCulture("zh-CN")]
        public void WhenParsingYenYuanSymbolInChina_ThenThisShouldReturnChineseYuan()
        {
            var yuan = Amount.Parse("¥ 765");

            yuan.Currency.Code.Should().Be("CNY");

            yuan.Should().Be(new Amount(765m, "CNY"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingYenYuanInNetherlands_ThenThisShouldFail()
        {
            // ¥ symbol is used for Japanese yen and Chinese yuan
            Action action = () => Amount.Parse("¥ 765");

            action.Should().Throw<FormatException>().WithMessage("*multiple known currencies*");
        }

        [Fact, UseCulture("en-US")]
        public void WhenParsingDollarSymbolInUSA_ThenThisShouldReturnUSDollar()
        {
            var dollar = Amount.Parse("$765.43");

            dollar.Currency.Code.Should().Be("USD");

            dollar.Should().Be(new Amount(765.43m, "USD"));
        }

        [Fact, UseCulture("es-AR")]
        public void WhenParsingDollarSymbolInArgentina_ThenThisShouldReturnArgentinePeso()
        {
            var peso = Amount.Parse("$765,43");

            peso.Should().Be(new Amount(765.43m, "ARS"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingDollarSymbolInNetherlands_ThenThisShouldFail()
        {
            // $ symbol is used for multiple currencies
            Action action = () => Amount.Parse("$ 765,43");

            action.Should().Throw<FormatException>().WithMessage("*multiple known currencies*");
        }

        [Fact, UseCulture("en-US")]
        public void WhenParsingEuroSymbolInUSA_ThenThisShouldReturnUSDollar()
        {
            var euro = Amount.Parse("€765.43");

            euro.Should().Be(new Amount(765.43m, "EUR"));
        }

        [Fact]
        public void WhenValueIsNull_ThenThowExeception()
        {
            Action action = () => Amount.Parse(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void WhenValueIsEmpty_ThenThowExeception()
        {
            Action action = () => Amount.Parse("");

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenCurrencyIsUnknown_ThenThowExeception()
        {
            Action action = () => Amount.Parse("XYZ 765,43");

            action.Should().Throw<FormatException>().WithMessage("*unknown currency*");
        }
    }

    public class GivenIWantToParseExplicitCurrency
    {
        public GivenIWantToParseExplicitCurrency()
        {
            _ = Currency.GetAllCurrencies();
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingYenInNetherlands_ThenThisShouldSucceed()
        {
            var yen = Amount.Parse("¥ 765", Currency.FromCode("JPY"));

            yen.Should().Be(new Amount(765, "JPY"));
        }

        [Fact, UseCulture("en-US")]
        public void WhenParsingArgentinePesoInUSA_ThenThisShouldReturnArgentinePeso()
        {
            var peso = Amount.Parse("$765.43", Currency.FromCode("ARS"));

            peso.Should().Be(new Amount(765.43m, "ARS"));
        }

        [Fact, UseCulture("es-AR")]
        public void WhenParsingUSDollarSymbolInArgentina_ThenThisShouldReturnUSDollar()
        {
            var dollar = Amount.Parse("$765,43", Currency.FromCode("USD"));

            dollar.Should().Be(new Amount(765.43m, "USD"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingUSDollarInNetherlands_ThenThisShouldSucceed()
        {
            // $ symbol is used for multiple currencies
            var dollar = Amount.Parse("$765,43", Currency.FromCode("USD"));

            dollar.Should().Be(new Amount(765.43m, "USD"));
        }

        [Fact, UseCulture("nl-BE")]
        public void WhenInBelgiumDutchSpeaking_ThenThisShouldSucceed()
        {
            var euro = Amount.Parse("€ 765,43", Currency.FromCode("EUR"));

            euro.Should().Be(new Amount(765.43m, "EUR"));
        }

        [Fact, UseCulture("fr-BE")]
        public void WhenInBelgiumFrenchSpeaking_ThenThisShouldSucceed()
        {
            var euro = Amount.Parse("765,43 €", Currency.FromCode("EUR"));

            euro.Should().Be(new Amount(765.43, "EUR"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingNumberWithoutCurrency_ThenThisShouldSucceed()
        {
            var euro = Amount.Parse("765,43", Currency.FromCode("USD"));

            euro.Should().Be(new Amount(765.43, "USD"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingUSDollarWithEuroCurrency_ThenThisShouldFail()
        {
            Action action = () => Amount.Parse("€ 765,43", Currency.FromCode("USD"));

            action.Should().Throw<FormatException>().WithMessage("Input string was not in a correct format.");                
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenValueIsNull_ThenThowExeception()
        {
            Action action = () => Amount.Parse(null, Currency.FromCode("EUR"));

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenValueIsEmpty_ThenThowExeception()
        {
            Action action = () => Amount.Parse("", Currency.FromCode("EUR"));

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenValueIsNullWithOverrideMethod_ThenThowExeception()
        {
            Action action = () => Amount.Parse(null, NumberStyles.Currency, null, Currency.FromCode("EUR"));

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenValueIsEmptyWithOverrideMethod_ThenThowExeception()
        {
            Action action = () => Amount.Parse("", NumberStyles.Currency, null, Currency.FromCode("EUR"));

            action.Should().Throw<ArgumentNullException>();
        }
    }

    public class GivenIWantToParseNegativeMoney
    {
        public GivenIWantToParseNegativeMoney()
        {
            _ = Currency.GetAllCurrencies();
        }

        [Fact, UseCulture("en-US")]
        public void WhenMinusSignBeforeDollarSign_ThenThisShouldSucceed()
        {
            var dollar = Amount.Parse("-$765.43");

            dollar.Should().Be(new Amount(-765.43, "USD"));
        }

        [Fact, UseCulture("en-US")]
        public void WhenMinusSignAfterDollarSign_ThenThisShouldSucceed()
        {
            var dollar = Amount.Parse("$-765.43");

            dollar.Should().Be(new Amount(-765.43, "USD"));
        }

        [Fact, UseCulture("en-US")]
        public void WhenDollarsWithParentheses_ThenThisShouldSucceed()
        {
            var dollar = Amount.Parse("($765.43)");

            dollar.Should().Be(new Amount(-765.43, "USD"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenMinusSignBeforeEuroSign_ThenThisShouldSucceed()
        {
            var value = "-€ 765,43";
            var dollar = Amount.Parse(value);

            dollar.Should().Be(new Amount(-765.43, "EUR"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenMinusSignAfterEuroSign_ThenThisShouldSucceed()
        {
            var value = "€ -765,43";
            var dollar = Amount.Parse(value);

            dollar.Should().Be(new Amount(-765.43, "EUR"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenEurosWithParentheses_ThenThisShouldSucceed()
        {
            var value = "(€ 765,43)";
            var dollar = Amount.Parse(value);

            dollar.Should().Be(new Amount(-765.43, "EUR"));
        }
    }

    public class GivenIWantToParseMoneyWithMoreDecimalPossibleForCurrency
    {
        public GivenIWantToParseMoneyWithMoreDecimalPossibleForCurrency()
        {
            _ = Currency.GetAllCurrencies();
        }

        [Fact, UseCulture("ja-JP")]
        public void WhenParsingJapaneseYen_ThenThisShouldBeRoundedDown()
        {
            var currency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            var yen = Amount.Parse("¥ 765.4",currency);

            yen.Should().Be(new Amount(765m, "JPY"));
        }

        [Fact, UseCulture("ja-JP")]
        public void WhenParsingJapaneseYen_ThenThisShouldBeRoundedUp()
        {
            var currency = Currency.FromCulture(Thread.CurrentThread.CurrentCulture);

            var yen = Amount.Parse("¥ 765.5",currency);

            yen.Should().Be(new Amount(766m, "JPY"));
        }
    }

    public class GivenIWantToTryParseImplicitCurrency
    {
        public GivenIWantToTryParseImplicitCurrency()
        {
            _ = Currency.GetAllCurrencies();
        }

        [Fact, UseCulture("nl-BE")]
        public void WhenInBelgiumDutchSpeaking_ThenThisShouldSucceed()
        {
            Amount.TryParse("€ 765,43", out var euro).Should().BeTrue();

            euro.Should().Be(new Amount(765.43m, "EUR"));
        }

        [Fact, UseCulture("fr-BE")]
        public void WhenInBelgiumFrenchSpeaking_ThenThisShouldSucceed()
        {
            Amount.TryParse("765,43 €", out var euro).Should().BeTrue();

            euro.Should().Be(new Amount(765.43, "EUR"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingNumberWithoutCurrency_ThenThisUseCurrencyOfCurrentCulture()
        {
            Amount.TryParse("765,43", out var euro).Should().BeTrue();

            euro.Should().Be(new Amount(765.43, "EUR"));
        }

        [Fact, UseCulture("ja-JP")]
        public void WhenParsingYenYuanSymbolInJapan_ThenThisShouldReturnJapaneseYen()
        {
            Amount.TryParse("¥ 765", out var yen).Should().BeTrue();

            yen.Should().Be(new Amount(765m, "JPY"));
        }

        [Fact, UseCulture("zh-CN")]
        public void WhenParsingYenYuanSymbolInChina_ThenThisShouldReturnChineseYuan()
        {
            Amount.TryParse("¥ 765", out var yuan).Should().BeTrue();

            yuan.Should().Be(new Amount(765m, "CNY"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingYenYuanInNetherlands_ThenThisShouldReturnFalse()
        {
            // ¥ symbol is used for Japanese yen and Chinese yuan
            Amount.TryParse("¥ 765", out var money).Should().BeFalse();

            money.Should().Be(new Amount(0m, Currency.FromCode("XXX")));
        }

        [Fact, UseCulture("en-US")]
        public void WhenParsingDollarSymbolInUSA_ThenThisShouldReturnUSDollar()
        {
            Amount.TryParse("$765.43", out var dollar).Should().BeTrue();

            dollar.Should().Be(new Amount(765.43m, "USD"));
        }

        [Fact, UseCulture("es-AR")]
        public void WhenParsingDollarSymbolInArgentina_ThenThisShouldReturnArgentinePeso()
        {
            Amount.TryParse("$765,43", out var peso).Should().BeTrue();

            peso.Should().Be(new Amount(765.43m, "ARS"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingDollarSymbolInNetherlands_ThenThisShouldReturnFalse()
        {
            // $ symbol is used for multiple currencies
            Amount.TryParse("$ 765,43", out var money).Should().BeFalse();

            money.Should().Be(new Amount(0m, Currency.FromCode("XXX")));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenValueIsNull_ThenReturnFalse()
        {
            Amount.TryParse(null, out var money).Should().BeFalse();

            money.Should().Be(new Amount(0m, Currency.FromCode("XXX")));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenValueIsEmpty_ThenReturnFalse()
        {
            Amount.TryParse("", out var money).Should().BeFalse();

            money.Should().Be(new Amount(0m, Currency.FromCode("XXX")));
        }
    }

    public class GivenIWantToTryParseExplicitCurrency
    {
        public GivenIWantToTryParseExplicitCurrency()
        {
            _ = Currency.GetAllCurrencies();
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingYenInNetherlands_ThenThisShouldSucceed()
        {
            Amount.TryParse("¥ 765", Currency.FromCode("JPY"), out var yen).Should().BeTrue();

            yen.Should().Be(new Amount(765, "JPY"));
        }

        [Fact, UseCulture("en-US")]
        public void WhenParsingArgentinePesoInUSA_ThenThisShouldReturnArgentinePeso()
        {
            Amount.TryParse("$765.43", Currency.FromCode("ARS"), out var peso).Should().BeTrue();

            peso.Should().Be(new Amount(765.43m, "ARS"));
        }

        [Fact, UseCulture("es-AR")]
        public void WhenParsingUSDollarSymbolInArgentina_ThenThisShouldReturnUSDollar()
        {
            Amount.TryParse("$765,43", Currency.FromCode("USD"), out var dollar).Should().BeTrue();

            dollar.Should().Be(new Amount(765.43m, "USD"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingUSDollarInNetherlands_ThenThisShouldSucceed()
        {
            // $ symbol is used for multiple currencies
            Amount.TryParse("$765,43", Currency.FromCode("USD"), out var dollar).Should().BeTrue();

            dollar.Should().Be(new Amount(765.43m, "USD"));
        }

        [Fact, UseCulture("nl-BE")]
        public void WhenInBelgiumDutchSpeaking_ThenThisShouldSucceed()
        {
            Amount.TryParse("€ 765,43", Currency.FromCode("EUR"), out var euro).Should().BeTrue();

            euro.Should().Be(new Amount(765.43m, "EUR"));
        }

        [Fact, UseCulture("fr-BE")]
        public void WhenInBelgiumFrenchSpeaking_ThenThisShouldSucceed()
        {
            Amount.TryParse("765,43 €", Currency.FromCode("EUR"), out var euro).Should().BeTrue();

            euro.Should().Be(new Amount(765.43, "EUR"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingNumberWithoutCurrency_ThenThisShouldSucceed()
        {
            Amount.TryParse("765,43", Currency.FromCode("USD"), out var euro).Should().BeTrue();

            euro.Should().Be(new Amount(765.43, "USD"));
        }

        [Fact, UseCulture("nl-NL")]
        public void WhenParsingUSDollarWithEuroCurrency_ThenThisShouldReturnFalse()
        {
            Amount.TryParse("€ 765,43", Currency.FromCode("USD"), out var money).Should().BeFalse();

            money.Should().Be(new Amount(0m, Currency.FromCode("XXX")));
        }
    }
}
