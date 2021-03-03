using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using Xunit;
using FluentAssertions;
using ApprovalTests.Reporters;
using ApprovalTests;

namespace OurPresence.Modeller.CoreTests
{
    [UseReporter(typeof(DiffReporter))]
    public class EnumerationFacts
    {
        [Fact]
        public void Enumeration_CanSerialize_Success()
        {
            var module = new Module("Company", "Test");
            var sut = new Enumeration("PaymentTypes")
                .AddItem("Cash")
                .AddItem("CreditCard")
                .AddItem("Cheque")
                .AddItem("DirectDebit");
            module.Enumerations.Add(sut);

            var expected = module.ToJson();

            Approvals.VerifyJson(expected);
            
            var actual = expected.FromJson<Module>();
            actual.Should().BeEquivalentTo(module);
        }

        [Fact]
        public void Enumeration_FlagValues_Set()
        {
            var module = new Module("Company", "Test");
            var sut = new Enumeration("PaymentTypes", true)
                .AddItem("None")
                .AddItem("Cash")
                .AddItem("CreditCard")
                .AddItem("Cheque")
                .AddItem("DirectDebit");
            module.Enumerations.Add(sut);

            var expected = module.ToJson();

            Approvals.VerifyJson(expected);

            var actual = expected.FromJson<Module>();
            actual.Should().BeEquivalentTo(module);
        }
    }
}
