using OurPresence.Modeller.Domain;

namespace OurPresence.Modeller.CoreFunctionalTests
{
    internal class Helpers
    {
        internal static Domain.Module CreateKeyModule()
        {
            var module = Fluent.Module
                .Create("Hy", "Modeller")
                .AddModel("Model")
                    .WithDefaultKey()
                    .AddField("Name").BusinessKey(true).MaxLength(5).Build
                    .AddRelationship().Relate("Model", new[] { "Id" }, "Key", new[] { "ModelId" }, RelationshipTypes.One, RelationshipTypes.One).Build
                    .Build
                .AddModel("Key")
                    .WithDefaultKey()
                    .AddField("ModelId").DataType(DataTypes.UniqueIdentifier).Nullable(true).Build
                    .AddField("FieldId").DataType(DataTypes.UniqueIdentifier).Nullable(true).Build
                    .AddField("Sequence").DataType(DataTypes.Int32).Build
                    .AddIndex("UXModelFieldSequence")
                        .AddField("ModelId").Build
                        .AddField("FieldId").Build
                        .AddField("Sequence").Build
                        .IsUnique().Build
                    .Build
                .AddModel("ModelField")
                    .WithDefaultKey()
                    .AddField("Name").BusinessKey(true).MaxLength(5).Nullable(false).Build
                    .AddRelationship().Relate("ModelField", new[] { "Id" }, "Key", new[] { "FieldId" }, RelationshipTypes.One, RelationshipTypes.Many).Build
                    .Build
                .Build;
            return module;
        }

        internal static Domain.Module CreateTestModule()
        {
            var module = OurPresence.Modeller.Fluent.Module
                .Create("Acme", "TestProject")
                .AddModel("StandAlone")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("Code").BusinessKey(true).MaxLength(5).MinLength(3).Build
                    .AddField("Name").Build
                    .AddField("IntValue").DataType(DataTypes.Int32).Build
                    .AddField("FloatValue").DataType(DataTypes.Decimal).Precision(4).Build
                    .AddField("BoolValue").DataType(DataTypes.Bool).Default("true").Build
                    .AddField("DateValue").DataType(DataTypes.DateTimeOffset).Build
                    .AddField("GuidValue").DataType(DataTypes.UniqueIdentifier).Nullable(true).Build
                    .Build

                .AddModel("Parent")
                    .WithDefaultKey()
                    .AddField("Code").BusinessKey(true).MaxLength(50).Build
                    .AddField("Description").MaxLength(5000).Nullable(true).Build
                    .AddRelationship().Relate("Parent", new[] { "Id" }, "Child", new[] { "FatherId" }).Build
                    .AddRelationship().Relate("Parent", new[] { "Id" }, "Child", new[] { "MotherId" }).Build
                    .Build
                .AddModel("Child")
                    .WithDefaultKey()
                    .AddField("FatherId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("MotherId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("Name").MaxLength(20).Build
                    .Build

                .AddModel("ManyLeft")
                    .WithDefaultKey()
                    .AddField("Code").BusinessKey(true).MaxLength(50).Build
                    .AddField("Description").MaxLength(5000).Nullable(true).Build
                    .AddRelationship().Relate("ManyLeft", new[] { "Id" }, "ManyRight", new[] { "Id" }, RelationshipTypes.Many, RelationshipTypes.Many).Build
                    .Build
                .AddModel("ManyRight")
                    .WithDefaultKey()
                    .AddField("Name").MaxLength(20).Build
                    .Build

                .AddModel("OneLeft")
                    .WithDefaultKey()
                    .AddField("Name1").MaxLength(20).Build
                    .AddRelationship().Relate("OneLeft", new[] { "Id" }, "OneRight", new[] { "Id" }, RelationshipTypes.One, RelationshipTypes.One).Build
                    .Build
                .AddModel("OneRight")
                    .WithDefaultKey()
                    .AddField("Name2").MaxLength(20).Build
                    .Build

                .AddModel("Composite")
                    .WithKey()
                        .AddField("Key1").MaxLength(20).Build
                        .AddField("Key2").MaxLength(40).Build
                        .Build
                    .AddField("Name").MaxLength(20).Build
                    .Build
                .Build;

            return module;
        }

        internal static Module CreateModule()
        {
            //todo: edit this method to create your test module
            var module = OurPresence.Modeller.Fluent.Module
                .Create("Jbssa", "FreightRates")
                .AddModel("BaseFeeType")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("Code").BusinessKey(true).MaxLength(50).Build
                    .AddField("Description").MaxLength(200).Build
                    .AddField("AllowedVariable").DataType(DataTypes.Int32).Build
                    .AddField("CurrencyId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("SortOrder").DataType(DataTypes.Int32).Build
                    .AddField("IsActive").DataType(DataTypes.Bool).Default("true").Build
                    .AddRelationship().Relate("Currency", new[] { "Id" }, "BaseFeeType", new[] { "CurrencyId" }).Build
                    .AddRelationship().Relate("BaseFeeType", new[] { "Id" }, "RateFee", new[] { "BaseFeeTypeId" }).Build
                    .Build
                .AddModel("Carrier")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("Code").BusinessKey(true).MaxLength(4).Build
                    .AddField("Name").MaxLength(255).Build
                    .AddField("IsActive").DataType(DataTypes.Bool).Default("true").Build
                    .AddRelationship().Relate("Carrier", new[] { "Id" }, "Rate", new[] { "CarrierId" }).Build
                    .Build
                .AddModel("ContainerType")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("Code").BusinessKey(true).MaxLength(5).Build
                    .AddField("Name").MaxLength(50).Build
                    .AddField("Size").DataType(DataTypes.Int32).Build
                    .AddField("Weight").DataType(DataTypes.Int32).Build
                    .AddField("IsActive").DataType(DataTypes.Bool).Default("true").Build
                    .AddRelationship().Relate("ContainerType", new[] { "Id" }, "Rate", new[] { "ContainerTypeId" }).Build
                    .AddRelationship().Relate("ContainerType", new[] { "Id" }, "FeeType", new[] { "ContainerTypeId" }).Build
                    .Build
                .AddModel("Country")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("Code").BusinessKey(true).MaxLength(2).Build
                    .AddField("MasterCountryId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("RegionId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("SortOrder").DataType(DataTypes.Int32).Build
                    .AddField("IsActive").DataType(DataTypes.Bool).Default("true").Build
                    .AddRelationship().Relate("Country", new[] { "Id" }, "TransportLocation", new[] { "CountryId" }).Build
                    .AddRelationship().Relate("Region", new[] { "Id" }, "Country", new[] { "RegionId" }).Build
                    .Build
                .AddModel("Currency")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("Code").BusinessKey(true).MaxLength(3).Build
                    .AddField("MasterCurrencyId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("Description").MaxLength(1000).Build
                    .AddField("IsDefault").DataType(DataTypes.Bool).Build
                    .AddField("SortOrder").DataType(DataTypes.Int32).Build
                    .AddField("IsActive").DataType(DataTypes.Bool).Default("true").Build
                    .AddRelationship().Relate("Currency", new[] { "Id" }, "ExchangeRate", new[] { "CurrencyFromId" }).Build
                    .AddRelationship().Relate("Currency", new[] { "Id" }, "ExchangeRate", new[] { "CurrencyToId" }).Build
                    .AddRelationship().Relate("Currency", new[] { "Id" }, "FeeType", new[] { "CurrencyId" }).Build
                    .AddRelationship().Relate("Currency", new[] { "Id" }, "FeeValue", new[] { "CurrencyId" }).Build
                    .Build
                .AddModel("ExchangeRate")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("CurrencyFromId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("CurrencyToId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("EffectiveDate").DataType(DataTypes.DateTimeOffset).Build
                    .AddField("Price").DataType(DataTypes.Decimal).Scale(12).Precision(2).Build
                    .AddRelationship().Relate("Currency", new[] { "Id" }, "ExchangeRate", new[] { "CurrencyFromId" }).Build
                    .AddRelationship().Relate("Currency", new[] { "Id" }, "ExchangeRate", new[] { "CurrencyToId" }).Build
                    .AddIndex("UX_CurrencyFromToDate")
                        .AddField("CurrencyFromId").Build
                        .AddField("CurrencyToId").Build
                        .AddField("EffectiveDate").Build
                        .IsUnique()
                        .Build
                    .AddIndex("IX_EffectiveDate")
                        .AddField("EffectiveDate").Build
                        .IsClustered()
                        .Build
                    .Build
                .AddModel("FeeType")
                    .WithDefaultKey()
                    .AddField("BaseFeeTypeId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("CurrencyId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("CarrierId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("ContainerTypeId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("DestinationRegionId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("DestinationCountryId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("DestinationTransportLocationId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("OriginRegionId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("OriginCountryId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("OriginTransportLocationId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddRelationship().Relate("BaseFeeType", new[] { "Id" }, "FeeType", new[] { "BaseFeeTypeId" }).Build
                    .AddRelationship().Relate("Currency", new[] { "Id" }, "FeeType", new[] { "CurrencyId" }).Build
                    .AddRelationship().Relate("Carrier", new[] { "Id" }, "FeeType", new[] { "CarrierId" }).Build
                    .AddRelationship().Relate("ContainerType", new[] { "Id" }, "FeeType", new[] { "ContainerTypeId" }).Build
                    .AddRelationship().Relate("Region", new[] { "Id" }, "FeeType", new[] { "DestinationRegionId" }).Build
                    .AddRelationship().Relate("Country", new[] { "Id" }, "FeeType", new[] { "DestinationCountryId" }).Build
                    .AddRelationship().Relate("TransportLocation", new[] { "Id" }, "FeeType", new[] { "DestinationTransportLocationId" }).Build
                    .AddRelationship().Relate("Region", new[] { "Id" }, "FeeType", new[] { "OriginRegionId" }).Build
                    .AddRelationship().Relate("Country", new[] { "Id" }, "FeeType", new[] { "OriginCountryId" }).Build
                    .AddRelationship().Relate("TransportLocation", new[] { "Id" }, "FeeType", new[] { "OriginTransportLocationId" }).Build
                    .AddRelationship().Relate("FeeType", new[] { "Id" }, "FeeValue", new[] { "FeeTypeId" }).Build
                    .AddIndex("UX_FeeTypes")
                        .AddField("BaseFeeTypeId").Build
                        .AddField("CurrencyId").Build
                        .AddField("CarrierId").Build
                        .AddField("ContainerTypeId").Build
                        .AddField("DestinationRegionId").Build
                        .AddField("DestinationCountryId").Build
                        .AddField("DestinationTransportLocationId").Build
                        .AddField("OriginRegionId").Build
                        .AddField("OriginCountryId").Build
                        .AddField("OriginTransportLocationId").Build
                        .IsUnique()
                        .Build
                    .Build
                .AddModel("FeeValue")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("CurrencyId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("FeeTypeId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("EffectiveDate").DataType(DataTypes.DateTimeOffset).Build
                    .AddField("Expired").DataType(DataTypes.Bool).Build
                    .AddField("ExpiryDate").DataType(DataTypes.DateTimeOffset).Build
                    .AddField("SiteId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("Value").DataType(DataTypes.Int32).Build
                    .AddRelationship().Relate("FeeType", new[] { "Id" }, "FeeValue", new[] { "FeeTypeId" }).Build
                    .AddRelationship().Relate("Currency", new[] { "Id" }, "FeeValue", new[] { "CurrencyId" }).Build
                    .Build
                .AddModel("Rate")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("Uid").BusinessKey(true).MaxLength(15).Build
                    .AddField("CarrierId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("ContainerTypeId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("GroupCode").MaxLength(5).Nullable(true).Build
                    .AddField("Notes").MaxLength(4000).Nullable(true).Build
                    .AddField("SiteId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("IsActive").DataType(DataTypes.Bool).Default("true").Build
                    .AddRelationship().Relate("Carrier", new[] { "Id" }, "Rate", new[] { "CarrierId" }).Build
                    .AddRelationship().Relate("ContainerType", new[] { "Id" }, "Rate", new[] { "ContainerTypeId" }).Build
                    .AddIndex("UX_Rate")
                        .AddField("CarrierId").Build
                        .AddField("ContainerTypeId").Build
                        .AddField("GroupCode").Build
                        .AddField("SiteId").Build
                        .IsUnique()
                        .Build
                    .Build
                .AddModel("RateFee")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("BaseFeeTypeId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("RateId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("EffectiveDate").DataType(DataTypes.DateTimeOffset).Build
                    .AddField("IsActive").DataType(DataTypes.Bool).Default("true").Build
                    .AddRelationship().Relate("Rate", new[] { "Id" }, "RateFee", new[] { "RateId" }).Build
                    .AddRelationship().Relate("BaseFeeType", new[] { "Id" }, "Rate", new[] { "BaseFeeTypeId" }).Build
                    .Build
                .AddModel("Region")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("Code").BusinessKey(true).MaxLength(4).Build
                    .AddField("Name").MaxLength(50).Build
                    .AddField("IsActive").DataType(DataTypes.Bool).Default("true").Build
                    .AddRelationship().Relate("Region", new[] { "Id" }, "Country", new[] { "RegionId" }).Build
                    .AddRelationship().Relate("Region", new[] { "Id" }, "FeeType", new[] { "OriginRegionId" }).Build
                    .AddRelationship().Relate("Region", new[] { "Id" }, "FeeType", new[] { "DestinationRegionId" }).Build
                    .Build
                .AddModel("Route")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("DestinationId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("OriginId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("RateId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("Sequence").DataType(DataTypes.Int32).Build
                    .AddField("Tranship").MaxLength(500).Nullable(true).Build
                    .AddField("TransitTime").MaxLength(500).Nullable(true).Build
                    .AddField("IsActive").DataType(DataTypes.Bool).Default("true").Build
                    .AddField("IsDefault").DataType(DataTypes.Bool).Build
                    .AddRelationship().Relate("TransportLocation", new[] { "Id" }, "Route", new[] { "DestinationId" }).Build
                    .AddRelationship().Relate("TransportLocation", new[] { "Id" }, "Route", new[] { "OriginId" }).Build
                    .AddRelationship().Relate("Route", new[] { "Id" }, "RouteDetail", new[] { "RouteId" }).Build
                    .AddRelationship().Relate("Rate", new[] { "Id" }, "Route", new[] { "RateId" }).Build
                    .Build
                .AddModel("RouteDetail")
                    .WithDefaultKey()
                    .AddField("RouteId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("TransportLocationId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("Sequence").DataType(DataTypes.Int32).Build
                    .AddRelationship().Relate("Route", new[] { "Id" }, "RouteDetail", new[] { "RouteId" }).Build
                    .AddRelationship().Relate("TransportLocation", new[] { "Id" }, "RouteDetail", new[] { "TransportLocationId" }).Build
                    .Build
                .AddModel("TransportLocation")
                    .WithDefaultKey()
                    .IsAuditable(true)
                    .AddField("Code").BusinessKey(true).MaxLength(5).Build
                    .AddField("Name").MaxLength(255).Build
                    .AddField("CountryId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("IsActive").DataType(DataTypes.Bool).Default("true").Build
                    .AddRelationship().Relate("TransportLocation", new[] { "Id" }, "Route", new[] { "DestinationId" }).Build
                    .AddRelationship().Relate("TransportLocation", new[] { "Id" }, "Route", new[] { "OriginId" }).Build
                    .AddRelationship().Relate("Country", new[] { "Id" }, "TransportLocation", new[] { "CountryId" }).Build
                    .Build
                .Build;

            return module;
        }
    }
}
