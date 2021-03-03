using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using Xunit;
using FluentAssertions;

namespace OurPresence.Modeller.CoreTests
{
    public static class FieldExtensionFacts
    {
        [Theory]
        [InlineData(true, true, true, "bool?")]
        [InlineData(true, false, true, "bool")]
        [InlineData(true, true, false, "bool?")]
        [InlineData(true, false, false, "bool")]
        [InlineData(false, true, true, "bool")]
        [InlineData(false, false, true, "bool")]
        [InlineData(false, true, false, "bool")]
        [InlineData(false, false, false, "bool")]
        public static void BoolFieldDataType_ReturnsExpected(bool nullable, bool showNullable, bool guidnullable, string expected)
        {
            var field = new Field("Test") { DataType = DataTypes.Bool, Nullable = nullable };
            field.GetDataType(showNullable, guidnullable).Should().Be(expected);
        }

        [Theory]
        [InlineData(DataTypes.Date, true, true, true, "Date?")]
        [InlineData(DataTypes.Date, true, false, true, "Date")]
        [InlineData(DataTypes.Date, true, true, false, "Date?")]
        [InlineData(DataTypes.Date, true, false, false, "Date")]
        [InlineData(DataTypes.Date, false, true, true, "Date")]
        [InlineData(DataTypes.Date, false, false, true, "Date")]
        [InlineData(DataTypes.Date, false, true, false, "Date")]
        [InlineData(DataTypes.Date, false, false, false, "Date")]
        [InlineData(DataTypes.Time, true, true, true, "TimeSpan?")]
        [InlineData(DataTypes.Time, true, false, true, "TimeSpan")]
        [InlineData(DataTypes.Time, true, true, false, "TimeSpan?")]
        [InlineData(DataTypes.Time, true, false, false, "TimeSpan")]
        [InlineData(DataTypes.Time, false, true, true, "TimeSpan")]
        [InlineData(DataTypes.Time, false, false, true, "TimeSpan")]
        [InlineData(DataTypes.Time, false, true, false, "TimeSpan")]
        [InlineData(DataTypes.Time, false, false, false, "TimeSpan")]
        [InlineData(DataTypes.DateTime, true, true, true, "DateTime?")]
        [InlineData(DataTypes.DateTime, true, false, true, "DateTime")]
        [InlineData(DataTypes.DateTime, true, true, false, "DateTime?")]
        [InlineData(DataTypes.DateTime, true, false, false, "DateTime")]
        [InlineData(DataTypes.DateTime, false, true, true, "DateTime")]
        [InlineData(DataTypes.DateTime, false, false, true, "DateTime")]
        [InlineData(DataTypes.DateTime, false, true, false, "DateTime")]
        [InlineData(DataTypes.DateTime, false, false, false, "DateTime")]
        [InlineData(DataTypes.DateTimeOffset, true, true, true, "DateTimeOffset?")]
        [InlineData(DataTypes.DateTimeOffset, true, false, true, "DateTimeOffset")]
        [InlineData(DataTypes.DateTimeOffset, true, true, false, "DateTimeOffset?")]
        [InlineData(DataTypes.DateTimeOffset, true, false, false, "DateTimeOffset")]
        [InlineData(DataTypes.DateTimeOffset, false, true, true, "DateTimeOffset")]
        [InlineData(DataTypes.DateTimeOffset, false, false, true, "DateTimeOffset")]
        [InlineData(DataTypes.DateTimeOffset, false, true, false, "DateTimeOffset")]
        [InlineData(DataTypes.DateTimeOffset, false, false, false, "DateTimeOffset")]
        public static void DateFieldDataType_ReturnsExpected(DataTypes dataType, bool nullable, bool showNullable, bool guidnullable, string expected)
        {
            var field = new Field("Test") { DataType = dataType, Nullable = nullable };
            field.GetDataType(showNullable, guidnullable).Should().Be(expected);
        }

        [Theory]
        [InlineData(true, true, true, "Guid?")]
        [InlineData(true, false, true, "Guid")]
        [InlineData(true, true, false, "Guid?")]
        [InlineData(true, false, false, "Guid")]
        [InlineData(false, true, true, "Guid?")]
        [InlineData(false, false, true, "Guid")]
        [InlineData(false, true, false, "Guid")]
        [InlineData(false, false, false, "Guid")]
        public static void GuidFieldDataType_ReturnsExpected(bool nullable, bool showNullable, bool guidnullable, string expected)
        {
            var field = new Field("Test") { DataType = DataTypes.UniqueIdentifier, Nullable = nullable };
            field.GetDataType(showNullable, guidnullable).Should().Be(expected);
        }

        [Theory]
        [InlineData(true, true, true, "int?")]
        [InlineData(true, false, true, "int")]
        [InlineData(true, true, false, "int?")]
        [InlineData(true, false, false, "int")]
        [InlineData(false, true, true, "int")]
        [InlineData(false, false, true, "int")]
        [InlineData(false, true, false, "int")]
        [InlineData(false, false, false, "int")]
        public static void IntFieldDataType_ReturnsExpected(bool nullable, bool showNullable, bool guidnullable, string expected)
        {
            var field = new Field("Test") { DataType = DataTypes.Int32, Nullable = nullable };
            field.GetDataType(showNullable, guidnullable).Should().Be(expected);
        }

        [Theory]
        [InlineData(2, true, true, true, "decimal?")]
        [InlineData(2, true, false, true, "decimal")]
        [InlineData(2, true, true, false, "decimal?")]
        [InlineData(2, true, false, false, "decimal")]
        [InlineData(2, false, true, true, "decimal")]
        [InlineData(2, false, false, true, "decimal")]
        [InlineData(2, false, true, false, "decimal")]
        [InlineData(2, false, false, false, "decimal")]
        public static void DecimalFieldDataType_ReturnsExpected(int scale, bool nullable, bool showNullable, bool guidnullable, string expected)
        {
            var field = new Field("Test") { DataType = DataTypes.Decimal, Scale = scale, Nullable = nullable };
            field.GetDataType(showNullable, guidnullable).Should().Be(expected);
        }

        [Theory]
        [InlineData(true, true, true, "string?")]
        [InlineData(true, false, true, "string")]
        [InlineData(true, true, false, "string?")]
        [InlineData(true, false, false, "string")]
        [InlineData(false, true, true, "string")]
        [InlineData(false, false, true, "string")]
        [InlineData(false, true, false, "string")]
        [InlineData(false, false, false, "string")]
        public static void StringFieldDataType_ReturnsExpected(bool nullable, bool showNullable, bool guidnullable, string expected)
        {
            var field = new Field("Test") { DataType = DataTypes.String, Nullable = nullable };
            field.GetDataType(showNullable, guidnullable).Should().Be(expected);
        }

        [Theory]
        [InlineData(null, true, true, true, "object")]
        [InlineData(null, true, false, true, "object")]
        [InlineData(null, true, true, false, "object")]
        [InlineData(null, true, false, false, "object")]
        [InlineData(null, false, true, true, "object")]
        [InlineData(null, false, false, true, "object")]
        [InlineData(null, false, true, false, "object")]
        [InlineData(null, false, false, false, "object")]
        [InlineData("MyObject", true, true, true, "MyObject")]
        [InlineData("MyObject", true, false, true, "MyObject")]
        [InlineData("MyObject", true, true, false, "MyObject")]
        [InlineData("MyObject", true, false, false, "MyObject")]
        [InlineData("MyObject", false, true, true, "MyObject")]
        [InlineData("MyObject", false, false, true, "MyObject")]
        [InlineData("MyObject", false, true, false, "MyObject")]
        [InlineData("MyObject", false, false, false, "MyObject")]
        public static void ObjectFieldDataType_ReturnsExpected(string dataTypeTypeName, bool nullable, bool showNullable, bool guidnullable, string expected)
        {
            var field = new Field("Test") { DataType = DataTypes.Object, DataTypeTypeName = dataTypeTypeName, Nullable = nullable };
            field.GetDataType(showNullable, guidnullable).Should().Be(expected);
        }
    }
}
