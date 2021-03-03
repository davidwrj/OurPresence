using OurPresence.Modeller.Domain;
using Xunit;
using OurPresence.Modeller.Domain.Validators;
using FluentValidation.TestHelper;

namespace OurPresence.Modeller.CoreTests
{
    public class FieldValidatorFacts
    {
        [Fact]
        public void Field_IsValid_ReturnsErrorWithNegativeDecimals()
        {
            var model = new Field("Test") { Scale = -3, DataType = DataTypes.Decimal };

            var sut = new FieldValidator();
            sut.ShouldHaveValidationErrorFor(m => m.Scale, model);
        }

        [Fact]
        public void Field_IsValid_ReturnsErrorWithDecimalsOnStringDataType()
        {
            var model = new Field("Test") { Scale = 3 };
            var sut = new FieldValidator();
            sut.ShouldHaveValidationErrorFor(m => m.Scale, model);
        }

        [Fact]
        public void Field_IsValid_ReturnsErrorWithNegativeMaxLength()
        {
            var model = new Field("Test") { MaxLength = -3 };

            var sut = new FieldValidator();
            sut.ShouldHaveValidationErrorFor(m => m.MaxLength, model);
        }

        [Fact]
        public void Field_IsValid_ReturnsErrorWithNegativeMinLength()
        {
            var model = new Field("Test") { MinLength = -3 };

            var sut = new FieldValidator();
            sut.ShouldHaveValidationErrorFor(m => m.MinLength, model);
        }

        [Fact]
        public void Field_IsValid_ReturnsErrorWhenMinLengthGreaterThenMaxLength()
        {
            var model = new Field("Test") { MinLength = 45, MaxLength = 5 };

            var sut = new FieldValidator();
            sut.ShouldHaveValidationErrorFor(m => m.MinLength, model);
        }
    }
}
