using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using Xunit;
using FluentAssertions;

namespace OurPresence.Modeller.CoreTests
{
    public static class ModelExtensionFacts
    {
        [Fact]
        public static void Model_HasActive_ReturnsTrueIfHasActive()
        {
            var sut = new Model("Test");
            sut.Fields.Add(new Field("IsActive"));
            sut.HasActive().Should().BeTrue();
        }

        [Fact]
        public static void Model_HasActive_ReturnsFalseIfNoActive()
        {
            var sut = new Model("Test");
            sut.Fields.Add(new Field("IsComplete"));
            sut.HasActive().Should().BeFalse();
        }

        [Fact]
        public static void Model_HasBusinessKey_ReturnsTrueIfHasBusinessKey()
        {
            var sut = new Model("Test");
            sut.Fields.Add(new Field("Code") { BusinessKey = true });
            sut.HasBusinessKey().Should().NotBeNull();
        }

        [Fact]
        public static void Model_HasBusinessKey_ReturnsFalseIfNoBusinessKey()
        {
            var sut = new Model("Test");
            sut.Fields.Add(new Field("Code"));
            sut.HasBusinessKey().Should().BeNull();
        }

        [Fact]
        public static void Model_IsEntity_ReturnsTrue()
        {
            var sut = new Model("Test");
            sut.MakeEntity();
            sut.Fields.Add(new Field("IsActive"));
            sut.IsEntity().Should().BeTrue();
        }

        [Fact]
        public static void Model_IsEntity_ReturnsFalse()
        {
            var sut = new Model("Test");
            sut.Fields.Add(new Field("IsComplete"));
            sut.IsEntity().Should().BeFalse();
        }

        [Fact]
        public static void Model_MakeEntity_WhenItHasAKey()
        {
            var sut = new Model("Test");
            sut.Key.Fields.Add(new Field("MyKey"));

            sut.MakeEntity();

            sut.Key.Fields.Should().BeEquivalentTo(new Field("Id") { DataType = DataTypes.UniqueIdentifier, Nullable = false });
        }

        [Fact]
        public static void Model_MakeEntity_WhenItHasAStringKey()
        {
            var sut = new Model("Test");
            sut.Key.Fields.Add(new Field("Id") { DataType = DataTypes.String });

            sut.MakeEntity();

            sut.Key.Fields.Should().BeEquivalentTo(new Field("Id") { DataType = DataTypes.UniqueIdentifier, Nullable = false });
        }

        [Fact]
        public static void Model_MakeEntity_WhenItHasACompositeKey()
        {
            var sut = new Model("Test");
            sut.Key.Fields.Add(new Field("Id") { DataType = DataTypes.UniqueIdentifier });
            sut.Key.Fields.Add(new Field("Id2") { DataType = DataTypes.String });

            sut.MakeEntity();

            sut.Key.Fields.Should().BeEquivalentTo(new Field("Id") { DataType = DataTypes.UniqueIdentifier, Nullable = false });
        }
    }
}
