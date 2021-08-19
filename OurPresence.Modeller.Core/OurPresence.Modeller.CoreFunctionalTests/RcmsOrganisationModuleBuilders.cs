// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;

namespace OurPresence.Modeller.CoreFunctionalTests
{
    public static class RcmsOrganisationModuleBuilders
    {
        public static Domain.Module CreateProject()
        {
            var mb = Fluent.Module.Create("Nhvr", "Rcms[Rcms]", "Organisation");

            return mb
                .AddOrganisationTypes()
                .AddOrganisation()
                .AddOrganisationKey()
                .AddAddressBook()
                .AddMapDetail()
                .AddOrganisationAddress()
                .AddOrganisationContact()
                .AddOrganisationDetail()
                .Build;
        }

        private static Fluent.ModuleBuilder AddOrganisationTypes(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddEnumeration("OrganisationTypes")
                    .AddItem("Company", 130)
                    .AddItem("IncorporatedAssociation", 131)
                    .AddItem("GovernmentDepartment", 132)
                    .AddItem("GovernmentBusinessEnterprise", 133)
                    .AddItem("LocalGovernment", 134)
                    .AddItem("Association", 139)
                .Build;
        }

        private static Fluent.ModuleBuilder AddOrganisation(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Organisation")
                .WithDefaultKey()
                .IsRoot()
                .AddField("CreationId").DataType(DataTypes.UniqueIdentifier).Nullable().Build
                .AddField("Name").MaxLength(255).Build
                .AddField("ACN").MaxLength(255).BusinessKey(true).Build
                .AddField("TypeId").DataType(DataTypes.Object).DataTypeTypeName("OrganisationTypes").Build
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build
                .AddField("SystemId").DataType(DataTypes.Int32).Default("30").Build
                .AddRelationship().Relate("Organisation", new[] { "Id" }, "OrganisationKey", new[] { "OrganisationId" }).Build
                .AddRelationship().Relate("Organisation", new[] { "Id" }, "OrganisationAddress", new[] { "OrganisationId" }).Build
                .AddRelationship().Relate("Organisation", new[] { "Id" }, "OrganisationContact", new[] { "OrganisationId" }).Build
                .AddBehaviour("Search")
                    .AddRequest("OrganisationSearchRequest")
                        .AddField("Name").Nullable().Build
                        .AddField("ACN").Build
                    .Build
                    .AddResponse("OrganisationSearchResult")
                        .IsCollection()
                        .AddField("Name").Nullable().Build
                        .AddField("ACN").Build
                        .AddField("AlertLevel").DataType(DataTypes.Int32).Nullable().Build
                        .AddField("AssociatedAlertLevel").DataType(DataTypes.Int32).Nullable().Build
                        .AddField("Source").Build
                        .AddField("Type").Build
                        .AddField("Status").Build
                    .Build
                .Build
                .AddBehaviour("index")
                    .AddRequest("OrganisationByIdRequest")
                        .AddField("Id").Build
                        .AddField("Juro").Build
                        .AddField("IncludeSilentAlerts").DataType(DataTypes.Bool).Default("false").Build
                    .Build
                    .AddResponse("OrganisationDetailResult")
                        .AddField("Name").Build
                        .AddField("ACN").Build
                        .AddField("AlertLevel").DataType(DataTypes.Int32).Nullable().Build
                        .AddField("AssociatedAlertLevel").DataType(DataTypes.Int32).Nullable().Build
                        .AddField("Source").Build
                        .AddField("Type").Build
                        .AddField("Status").Build
                    .Build
                .Build
                .AddBehaviour("save")
                    .AddRequest("OrganisationSaveRequest")
                        .AddField("Name").Build
                        .AddField("ACN").Build
                        .AddField("TypeId").DataType(DataTypes.Object).DataTypeTypeName("OrganisationTypes").Build
                        .AddField("CreationId").DataType(DataTypes.UniqueIdentifier).Nullable().Build
                    .Build
                    .AddResponse("OrganisationSaveResult")
                        .AddField("Id").Build
                    .Build
                .Build
            .Build;
        }

        private static Fluent.ModuleBuilder AddOrganisationKey(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("OrganisationKey")
                .WithDefaultKey()
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("Key").MaxLength(255).Build
                .AddField("Value").MaxLength(255).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddOrganisationAddress(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("OrganisationAddress")
                .WithDefaultKey()
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("AddressTypeId").DataType(DataTypes.Int32).Build
                .AddField("LocalGovernmentArea").MaxLength(128).Build
                .AddField("LgaShortTitle").MaxLength(128).Build
                .AddField("Near").MaxLength(128).Build
                .AddField("CommonName").MaxLength(128).Build
                .AddField("Remarks").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("Source").MaxLength(128).Build
                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Nullable().Build

                .AddRelationship().Relate("OrganisationAddress", new[] { "Id" }, "AddressBook", new[] { "OrganisationAddressId" }).Build
                .AddRelationship().Relate("OrganisationAddress", new[] { "Id" }, "MapDetail", new[] { "OrganisationAddressId" }).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddAddressBook(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("AddressBook")
                .WithDefaultKey()
                .AddField("FloorNumber").MaxLength(20).Nullable().Build
                .AddField("UnitType").MaxLength(200).Nullable().Build
                .AddField("UnitNumber").MaxLength(20).Nullable().Build
                .AddField("StreetName").MaxLength(128).Nullable().Build
                .AddField("StreetType").MaxLength(128).Nullable().Build
                .AddField("StreetSuffix").MaxLength(200).Nullable().Build
                .AddField("StreetNumberFirst").MaxLength(50).Nullable().Build
                .AddField("StreetNumberFirstPrefix").MaxLength(50).Nullable().Build
                .AddField("StreetNumberFirstSuffix").MaxLength(50).Nullable().Build
                .AddField("StreetNumberLast").MaxLength(50).Nullable().Build
                .AddField("StreetNumberLastPrefix").MaxLength(50).Nullable().Build
                .AddField("StreetNumberLastSuffix").MaxLength(50).Nullable().Build
                .AddField("StreetDirectional").MaxLength(200).Nullable().Build
                .AddField("PostalContainer").MaxLength(255).Nullable().Build
                .AddField("PostalCode").MaxLength(20).Nullable().Build
                .AddField("Suburb").MaxLength(100).Nullable().Build
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("Country").MaxLength(128).Nullable().Build
                .AddField("FullAddress").MaxLength(1500).Nullable().Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddMapDetail(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("MapDetail")
                .WithDefaultKey()
                .AddField("Altitude").DataType(DataTypes.Decimal).Nullable().Build
                .AddField("AltitudeAccuracy").DataType(DataTypes.Decimal).Nullable().Build
                .AddField("HorizontalAccuracy").DataType(DataTypes.Decimal).Nullable().Build
                .AddField("Longitude").DataType(DataTypes.Decimal).Nullable().Build
                .AddField("Latitude").DataType(DataTypes.Decimal).Nullable().Build
                .AddField("Elevation").DataType(DataTypes.Decimal).Nullable().Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddOrganisationContact(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("OrganisationContact")
                .WithDefaultKey()
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("ContactTypeId").DataType(DataTypes.Int32).Build
                .AddField("ContactValue").MaxLength(128).Build
                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Nullable().Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("Source").MaxLength(128).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddOrganisationDetail(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("OrganisationDetail")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("Name").MaxLength(128).Build
                .AddField("Acn").MaxLength(128).Build
                .AddField("IncorporatedNumber").MaxLength(128).Build
                .AddField("TypeId").DataType(DataTypes.Int32).Build
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("Status").MaxLength(128).Build
                .AddField("DateOfRegistration").DataType(DataTypes.DateTimeOffset).Nullable().Build
                .AddField("AlertLevel").DataType(DataTypes.Int32).Build
                .AddField("AssociatedAlertLevel").DataType(DataTypes.Int32).Build
                .AddField("Source").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }
    }
}
