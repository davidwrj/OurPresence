using OurPresence.Modeller.Domain;

namespace OurPresence.Modeller.CoreFunctionalTests
{
    public static class RcmsModuleBuilders
    {
        public static Domain.Module CreateProject()
        {
            var mb = Fluent.Module.Create("Nhvr", "Rcms[Rcms]");

            return mb
                .AddEnumStates()
                .AddEnumSourceSystemTypes()

                .AddActivityLog()
                .AddAddressBook()
                .AddAlert()

                .AddEvent()
                .AddEventAddress()
                .AddOrganisation()
                .AddPerson()
                .AddVehicle()
                .AddVehicleDetail()

                .Build;
        }

        private static Fluent.ModuleBuilder AddEnumStates(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddEnumeration("States")
                    .AddItem("ACT", 60, "Australian Capitol Territory")
                    .AddItem("WA", 61, "Western Australia")
                    .AddItem("SA", 62, "South Australia")
                    .AddItem("NT", 63, "Northern Territory")
                    .AddItem("VIC", 64, "Victoria")
                    .AddItem("QLD", 65, "Queensland")
                    .AddItem("NSW", 66, "New South Wales")
                    .AddItem("TAS", 67, "Tasmania")
                .Build;
        }

        private static Fluent.ModuleBuilder AddEnumSourceSystemTypes(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddEnumeration("SourceSystemTypes")
                    .AddItem("RCMS", 30)
                    .AddItem("NHVR", 31)
                    .AddItem("VCOM", 32)
                    .AddItem("INTERCEPT", 33)
                    .AddItem("VICROADSNHVR", 34)
                    .AddItem("VICROADS", 35)
                    .AddItem("ASIC", 36)
                    .AddItem("NEVDIS", 37)
                    .AddItem("QLDTMR", 38)
                    .AddItem("NEVDISSERVICE", 39)
                .Build;
        }

        private static Fluent.ModuleBuilder AddEvent(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Event")
                .WithDefaultKey()
                .AddField("CreationId").DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("StatusId").DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("OrganisationId").DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("ReviewedUserId").DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("ReviewedDateTime").DataType(Domain.DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("CreateDate").DataType(Domain.DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("CompletedDate").DataType(Domain.DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("ReportedOnDate").DataType(Domain.DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("TookPlaceStartDate").DataType(Domain.DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("TookPlaceEndDate").DataType(Domain.DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("VehicleReleaseDate").DataType(Domain.DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("VehicleGrounded").DataType(Domain.DataTypes.Bool).Nullable(true).Build
                .AddField("OperationName").MaxLength(255).Nullable(true).Build
                .AddField("MethodOfIntercept").MaxLength(255).Nullable(true).Build
                .AddField("DirectionOfTravel").MaxLength(255).Nullable(true).Build
                .AddField("Lights").DataType(Domain.DataTypes.Bool).Nullable(true).Build
                .AddField("Sirens").DataType(Domain.DataTypes.Bool).Nullable(true).Build
                .AddField("UrgentDutyDriving").DataType(Domain.DataTypes.Bool).Nullable(true).Build
                .AddField("ComplianceActionOffenceReport").DataType(Domain.DataTypes.Bool).Nullable(true).Build
                .AddField("ComplianceActionOffenceReportReference").DataType(Domain.DataTypes.Bool).Nullable(true).Build
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build
                .AddField("SystemId").DataType(Domain.DataTypes.Object).DataTypeTypeName("SourceSystemTypes").Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddEventAddress(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventAddress")
                .WithDefaultKey()
                .AddField("StateId").DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("EventAddressType").DataType(Domain.DataTypes.Object).DataTypeTypeName("AddressType").Nullable(true).Build
                .AddField("LocalGovernmentArea").MaxLength(255).Nullable(true).Build
                .AddField("LGAShortTitle").MaxLength(50).Nullable(true).Build
                .AddField("Near").MaxLength(255).Nullable(true).Build
                .AddField("CommonName").MaxLength(255).Nullable(true).Build
                .AddField("Remarks").MaxLength(255).Nullable(true).Build
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build
                .AddField("Altitude").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("AltitudeAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("HorizontalAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("Longitude").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("Latitude").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("Elevation").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("FloorNumber").MaxLength(20).Nullable(true).Build
                .AddField("UnitType").MaxLength(200).Nullable(true).Build
                .AddField("UnitNumber").MaxLength(20).Nullable(true).Build
                .AddField("StreetName").MaxLength(128).Nullable(true).Build
                .AddField("StreetType").MaxLength(128).Nullable(true).Build
                .AddField("StreetSuffix").MaxLength(200).Nullable(true).Build
                .AddField("StreetNumberFirst").MaxLength(50).Nullable(true).Build
                .AddField("StreetNumberFirstPrefix").MaxLength(50).Nullable(true).Build
                .AddField("StreetNumberFirstSuffix").MaxLength(50).Nullable(true).Build
                .AddField("StreetNumberLast").MaxLength(50).Nullable(true).Build
                .AddField("StreetNumberLastPrefix").MaxLength(50).Nullable(true).Build
                .AddField("StreetNumberLastSuffix").MaxLength(50).Nullable(true).Build
                .AddField("StreetDirectional").MaxLength(200).Nullable(true).Build
                .AddField("PostalContainer").MaxLength(255).Nullable(true).Build
                .AddField("PostalCode").MaxLength(20).Nullable(true).Build
                .AddField("Suburb").MaxLength(100).Nullable(true).Build
                .AddField("Country").MaxLength(128).Nullable(true).Build
                .AddField("FullAddress").MaxLength(1500).Nullable(true).Build
                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Build

                .AddRelationship()
                    .Relate("Event", new[] { "Id" }, "EventAddress", new[] { "EventId" }).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddOrganisation(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Organisation")
                .WithDefaultKey()
                .AddField("CreationId").DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("Name").MaxLength(255).Build
                .AddField("ACN").MaxLength(255).Nullable(true).Build
                .AddField("TypeId").DataType(Domain.DataTypes.Object).DataTypeTypeName("OrganisationType").Build
                .AddField("NevdisId").MaxLength(255).Nullable(true).Build
                .AddField("VicCustomerNo").MaxLength(255).Nullable(true).Build
                .AddField("ParentId").DataType(DataTypes.Int32).Nullable(true).Build
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build
                .AddField("SystemId").DataType(DataTypes.Int32).Default("30").Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddPerson(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Person")
                .WithDefaultKey()
                .AddField("CreationId").DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("GivenName").MaxLength(255).Build
                .AddField("FamilyName").MaxLength(255).Build
                .AddField("DateOfBirth").DataType(DataTypes.DateTimeOffset).Build
                .AddField("LicenceNumber").MaxLength(255).Nullable(true).Build
                .AddField("LicenceState").DataType(Domain.DataTypes.Object).DataTypeTypeName("States").Build
                .AddField("SystemId").DataType(DataTypes.Int32).Default("30").Build
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build
                .AddField("SourcePersonId").MaxLength(64).Nullable(true).Build
                .AddField("NevdisId").MaxLength(255).Nullable(true).Build
                .AddField("ParentId").DataType(DataTypes.Int32).Nullable(true).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddVehicle(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Vehicle")
                .WithDefaultKey()
                .AddField("CreationId").BusinessKey(true).DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("RegistrationState").DataType(Domain.DataTypes.Object).DataTypeTypeName("States")
                .Nullable(true).Build
                .AddField("RegistrationNumber").MaxLength(10).Nullable(true).Build
                .AddField("IsNationalRegistration").DataType(DataTypes.Bool).Build
                .AddField("VehicleIdentificationNumber").MaxLength(20).Nullable(true).Build
                .AddField("EngineNumber").MaxLength(20).Nullable(true).Build
                .AddField("ChassisNumber").MaxLength(32).Nullable(true).Build
                .AddField("SourceVehicleId").MaxLength(64).Build
                .AddField("SystemId").DataType(Domain.DataTypes.Object).DataTypeTypeName("SourceSystemTypes").Build
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build

                .AddBehaviour("Create")
                .Raising("Created")
                .AddField("RegistrationState").DataType(Domain.DataTypes.Object).DataTypeTypeName("States")
                .Nullable(true).Build
                .AddField("RegistrationNumber").MaxLength(10).Nullable(true).Build
                .AddField("IsNationalRegistration").DataType(DataTypes.Bool).Build
                .AddField("VehicleIdentificationNumber").MaxLength(20).Nullable(true).Build
                .AddField("EngineNumber").MaxLength(20).Nullable(true).Build
                .AddField("ChassisNumber").MaxLength(32).Nullable(true).Build
                .AddField("SourceVehicleId").MaxLength(64).Build
                .AddField("SystemId").DataType(Domain.DataTypes.Object).DataTypeTypeName("SourceSystemTypes").Build
                .Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddVehicleDetail(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("VehicleDetail")
                .WithDefaultKey()
                .AddField("VehicleIdentificationNumber").MaxLength(20).Nullable(true).Build
                .AddField("EngineNumber").MaxLength(20).Nullable(true).Build
                .AddField("ChassisNumber").MaxLength(32).Nullable(true).Build
                .AddField("RegistrationState").DataType(Domain.DataTypes.Object).DataTypeTypeName("States")
                .Nullable(true).Build
                .AddField("RegistrationNumber").MaxLength(10).Nullable(true).Build
                .AddField("RegistrationStatus").MaxLength(64).Nullable(true).Build
                .AddField("IsNationalRegistration").DataType(DataTypes.Bool).Build
                .AddField("SourceVehicleId").MaxLength(64).Build
                .AddField("SystemId").DataType(Domain.DataTypes.Object).DataTypeTypeName("SourceSystemTypes").Build
                .AddRelationship().Relate("Vehicle", new[] { "Id" }, "Event", new[] { "VehicleId" }).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddActivityLog(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ActivityLog")
                .WithDefaultKey()
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build
                .AddField("ActivityType").MaxLength(128).Build
                .AddField("EventId").Build
                .AddField("IsManualEntry").DataType(DataTypes.Bool).Default("true").Build
                .AddField("StartDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("EndDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("Lights").DataType(DataTypes.Bool).Build
                .AddField("Sirens").DataType(DataTypes.Bool).Build
                .AddField("UrgentDutyDriving").DataType(DataTypes.Bool).Build
                .AddField("Remarks").Build
                .AddField("ShiftId").Build
                .AddField("UserId").Build
                .AddRelationship().Relate("Event", new[] { "Id" }, "ActivityLog", new[] { "EventId" }).Build
                .AddRelationship().Relate("Shift", new[] { "Id" }, "ActivityLog", new[] { "ShiftId" }).Build
                .AddRelationship().Relate("User", new[] { "Id" }, "ActivityLog", new[] { "UserId" }).Build
                .AddIndex("IDX_ActivityItem_EndDate").AddField("EndDate").Build.Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddAddressBook(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("AddressBook")
                .WithDefaultKey()
                .AddField("UnitType").MaxLength(128).Build
                .AddField("UnitNumber").MaxLength(128).Build
                .AddField("StreetName").MaxLength(128).Build
                .AddField("StreetType").MaxLength(128).Build
                .AddField("StreetSuffix").MaxLength(128).Build
                .AddField("StreetNumberFirst").MaxLength(128).Build
                .AddField("StreetNumberFirstPrefix").MaxLength(128).Build
                .AddField("StreetNumberFirstSuffix").MaxLength(128).Build
                .AddField("StreetNumberLast").MaxLength(128).Build
                .AddField("StreetNumberLastPrefix").MaxLength(128).Build
                .AddField("StreetNumberLastSuffix").MaxLength(128).Build
                .AddField("StreetDirectional").MaxLength(128).Build
                .AddField("PostalContainer").MaxLength(128).Build
                .AddField("PostalCode").MaxLength(128).Build
                .AddField("Suburb").MaxLength(128).Build
                .AddField("Country").MaxLength(128).Build
                .AddField("FullAddress").MaxLength(128).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddAlert(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Alert")
                .WithDefaultKey()
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build
                .AddField("EffectFrom").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("EffectTo").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("Level").DataType(DataTypes.Int32).Build
                .AddField("Title").MaxLength(128).Build
                .AddField("Remarks").Build
                .AddField("ReasonForDeletion").MaxLength(128).Build
                .AddField("Silent").DataType(DataTypes.Bool).Build
                .AddField("SilentNotifyContactId").DataType(DataTypes.Int32).Build
                .AddField("PersonId").DataType(DataTypes.Int32).Build
                .AddField("VehicleId").DataType(DataTypes.Int32).Build
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("UserId").DataType(DataTypes.Int32).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build
                .AddField("IsManualEntry").DataType(DataTypes.Bool).Build

                .AddRelationship().Relate("Organisation", new[] { "Id" }, "Alert", new[] { "OrganisationId" }).Build
                .AddRelationship().Relate("Person", new[] { "Id" }, "Alert", new[] { "PersonId" }).Build
                .AddRelationship().Relate("User", new[] { "Id" }, "Alert", new[] { "UserId" }).Build
                .AddRelationship().Relate("Vehicle", new[] { "Id" }, "Alert", new[] { "VehicleId" }).Build
                .Build;
        }
    }
}
