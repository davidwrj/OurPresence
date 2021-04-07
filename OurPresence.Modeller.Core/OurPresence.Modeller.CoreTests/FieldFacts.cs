using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using Xunit;
using FluentAssertions;
using ApprovalTests;
using ApprovalTests.Reporters;

namespace OurPresence.Modeller.CoreTests
{
    [UseReporter(typeof(DiffReporter))]
    public static class FieldFacts
    {
        [Fact]
        public static void Field_SetsDefaults_WhenCreated()
        {
            var sut = new Field("Test");

            sut.Name.ToString().Should().Be("Test");
            sut.Default.Should().BeNull();
            sut.MaxLength.Should().BeNull();
            sut.MinLength.Should().BeNull();
            sut.Scale.Should().BeNull();
            sut.Precision.Should().BeNull();
            sut.DataType.Should().Be(DataTypes.String);
            sut.DataTypeTypeName.Should().BeNull();
            sut.Nullable.Should().BeFalse();
            sut.BusinessKey.Should().BeFalse();
        }

        [Fact]
        public static void Field_Serialization()
        {
            var sut = new Field("Test")
            {
                Default = "MyValue",
                MaxLength = 20,
                Nullable = true,
                DataType = DataTypes.String,
                BusinessKey = false
            };
            var json = sut.ToJson();

            Approvals.VerifyJson(json);

            var actual = json.FromJson<Field>();
            actual.Should().BeEquivalentTo(sut);
        }
    }
}
