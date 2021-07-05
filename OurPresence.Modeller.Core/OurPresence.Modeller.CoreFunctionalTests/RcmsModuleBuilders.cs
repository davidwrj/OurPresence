// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;

namespace OurPresence.Modeller.CoreFunctionalTests
{
    public static class RcmsModuleBuilders
    {
        public static Domain.Module CreateProject()
        {
            var mb = Fluent.Module.Create("Nhvr", "Rcms[Rcms]");

            return mb
                .AddAddressTypes()
                .AddEnumStates()
                .AddEnumSourceSystemTypes()
                .AddOrganisationTypes()

                .AddActivityLog()
                .AddAddressBook()
                .AddAlert()
                .AddAzureSqlMaintenanceLog()
                .AddBatchProcess()
                .AddBatchProcessTask()
                .AddBatchProcessTaskItem()
                .AddComplianceAction()
                .AddComplianceActionImport()
                .AddComplianceActionLocation()
                .AddComplianceActionOffence()
                .AddComplianceActionService()
                .AddComplianceActionServiceAddress()
                .AddComplianceActionStatus()
                .AddComplianceActionSubComponent()
                .AddComplianceDefect()
                .AddComplianceDefectSubComponent()
                .AddComplianceDirectionDimension()
                .AddComplianceDirectionDimensionsHardcopy()
                .AddComplianceDirectionFatigue()
                .AddComplianceDirectionToRectify()
                .AddComplianceEducation()
                .AddComplianceFormalCaution()
                .AddComplianceFormalWarning()
                .AddComplianceInfringement()
                .AddComplianceOfficialWarning()
                .AddComplianceReportForProsecution()
                .AddComplianceWAFormalCaution()
                .AddEvent()
                .AddEventAddress()
                .AddEventInspection()
                .AddEventInspectionAxleGroup()
                .AddEventInspectionAxleWeight()
                .AddEventInspectionAxleWeightPwd()
                .AddEventInspectionMovement()
                .AddEventMediaExternal()
                .AddEventMedium()
                .AddEventNote()
                .AddEventOfficer()
                .AddEventOrganisation()
                .AddEventPerson()
                .AddEventPersonDiary()
                .AddEventPersonFatigue()
                .AddEventVehicle()
                .AddEventVehicleConcession()
                .AddEventVehicleCondition()
                .AddEventVehicleScheme()
                .AddIntercept()
                .AddInterceptImport()
                .AddInterceptJourney()
                .AddInterceptOrganisation()
                .AddInterceptPerson()
                .AddInterceptVehicle()
                .AddLog()
                .AddMapDetail()
                .AddNoticeNumber()
                .AddOrganisation()
                .AddOrganisationAddress()
                .AddOrganisationContact()
                .AddOrganisationDetail()
                .AddPerson()
                .AddPersonAddress()
                .AddPersonContact()
                .AddPersonDetail()
                .AddPostcode()
                .AddReferenceData()
                .AddReferenceDataAttribute()
                .AddReferenceDataCategory()
                .AddReferenceDataMapping()
                .AddShift()
                .AddShiftOfficer()
                .AddUser()
                .AddVehicle()
                .AddVehicleDetail()
                .AddVehicleFootprint()
                .AddVehicleFootprintImport()
                .AddVehicleOrganisation()
                .AddVehiclePerson()
                .AddWAEventInspection()

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
        private static Fluent.ModuleBuilder AddAddressTypes(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddEnumeration("AddressTypes")
                    .AddItem("RegisteredAddress", 120)
                    .AddItem("PostalAddress", 121)
                    .AddItem("GaragingAddress", 122)
                    .AddItem("NoticeAddress", 123)
                    .AddItem("LastKnownAddress", 124)
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

        private static Fluent.ModuleBuilder AddActivityLog(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ActivityLog")
                .WithDefaultKey()
                .IsAuditable()
                .SupportCrud(CRUDSupport.All)
                .IsRoot()
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

                .AddBehaviour("Officer")
                    .AddResponse("ActivityLogOfficerResult")
                        .IsCollection()
                        .AddField("Id").DataType(DataTypes.Int32).Build
                        .AddField("StartTime").DataType(DataTypes.DateTimeOffset).Build
                        .AddField("EndTime").DataType(DataTypes.DateTimeOffset).Build
                        .AddField("Source").DataType(DataTypes.String).Build
                        .AddField("ActivityType").MaxLength(128).Build
                        .AddField("Remarks").Build
                        .AddField("Lights").DataType(DataTypes.Bool).Build
                        .AddField("Sirens").DataType(DataTypes.Bool).Build
                        .AddField("UrgentDutyDriving").DataType(DataTypes.Bool).Build
                        .AddField("IsEditable").DataType(DataTypes.Bool).Build
                        .AddField("SystemGenerated").DataType(DataTypes.Bool).Build
                        .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Build
                    .Build
                .Build

                .AddBehaviour("Add", BehaviourVerb.Post)
                    .AddRequest("ActivityLogAddRequest")                        
                        .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Build
                        .AddField("StartTime").DataType(DataTypes.DateTimeOffset).Build
                        .AddField("ActivityType").DataType(DataTypes.String).Build
                    .Build
                    .AddResponse("ActivityLogAddResult")
                        .AddField("Id").DataType(DataTypes.Int32).Build
                    .Build
                .Build

                .AddBehaviour("Update", BehaviourVerb.Post)
                    .AddRequest("ActivityLogUpdateRequest")
                        .AddField("EndTime").DataType(DataTypes.DateTimeOffset).Build
                        .AddField("Remarks").DataType(DataTypes.String).Build
                        .AddField("Lights").DataType(DataTypes.Bool).Nullable(true).Build
                        .AddField("Sirens").DataType(DataTypes.Bool).Nullable(true).Build
                        .AddField("UrgentDutyDriving").DataType(DataTypes.Bool).Nullable(true).Build
                    .Build
                    .AddResponse("ActivityLogUpdateResult")
                        .AddField("Id").DataType(DataTypes.Int32).Build
                    .Build
                .Build

                .AddBehaviour("Delete", BehaviourVerb.Delete)
                    .AddRequest("ActivityLogDeleteRequest")
                        .AddField("Id").DataType(DataTypes.Int32).Build
                    .Build
                .Build
            .Build;
        }

        private static Fluent.ModuleBuilder AddAddressBook(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("AddressBook")
                .WithDefaultKey()
                .AddField("FloorNumber").MaxLength(20).Nullable(true).Build
                .AddField("UnitType").MaxLength(128).Nullable(true).Build
                .AddField("UnitNumber").MaxLength(128).Nullable(true).Build
                .AddField("StreetName").MaxLength(128).Build
                .AddField("StreetType").MaxLength(128).Build
                .AddField("StreetSuffix").MaxLength(128).Build
                .AddField("StreetNumberFirst").MaxLength(128).Nullable(true).Build
                .AddField("StreetNumberFirstPrefix").MaxLength(128).Nullable(true).Build
                .AddField("StreetNumberFirstSuffix").MaxLength(128).Nullable(true).Build
                .AddField("StreetNumberLast").MaxLength(128).Nullable(true).Build
                .AddField("StreetNumberLastPrefix").MaxLength(128).Nullable(true).Build
                .AddField("StreetNumberLastSuffix").MaxLength(128).Nullable(true).Build
                .AddField("StreetDirectional").MaxLength(128).Nullable(true).Build
                .AddField("PostalContainer").MaxLength(128).Nullable(true).Build
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
                .IsAuditable()
                .IsRoot()
                .SupportCrud(CRUDSupport.Create | CRUDSupport.Delete)
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

                .AddBehaviour("Save", BehaviourVerb.Post)
                    .AddRequest("AlertSaveRequest")
                        .Route("")
                        .AddField("Id").DataType(DataTypes.Int32).Build
                        .AddField("AlertLevel").DataType(DataTypes.Int32).Build
                        .AddField("EffectiveDate").DataType(DataTypes.DateTimeOffset).Build
                        .AddField("ExpiryDate").DataType(DataTypes.DateTimeOffset).Build
                        .AddField("Remarks").Build
                        .AddField("Silent").DataType(DataTypes.Bool).Build
                        .AddField("SilentNotifyContactId").DataType(DataTypes.Int32).Build
                        .AddField("PersonId").DataType(DataTypes.Int32).Build
                        .AddField("VehicleId").DataType(DataTypes.Int32).Build
                        .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                    .Build
                    .AddResponse("AlertSaveResult")
                        .AddField("Id").DataType(DataTypes.Int32).Build
                    .Build
                .Build

                .AddBehaviour("Delete", BehaviourVerb.Delete)
                    .AddRequest("AlertDeleteRequest")
                        .AddField("Id").DataType(DataTypes.Int32).Build
                        .AddField("Reason").MaxLength(128).Build
                    .Build
                .Build
            .Build;
        }

        private static Fluent.ModuleBuilder AddAudit(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Audit")
                .SupportCrud(CRUDSupport.None)
                .IsRoot()
                .Build;
        }

        private static Fluent.ModuleBuilder AddAzureSqlMaintenanceLog(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("AzureSqlmaintenanceLog")
                .IsAuditable(false)
                .AddField("Id").DataType(DataTypes.Int64).Build
                .AddField("OperationTime").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("Command").MaxLength(128).Build
                .AddField("ExtraInfo").MaxLength(128).Build
                .AddField("StartTime").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("EndTime").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("StatusMessage").MaxLength(128).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddBatchProcess(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("BatchProcess")
                .WithDefaultKey()
                .IsAuditable()
                .IsRoot()
                .SupportCrud(CRUDSupport.None)
                .AddField("Url").MaxLength(128).Build
                .AddField("StatusId").DataType(DataTypes.Int32).Build
                .AddField("StartedOn").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("StoppedOn").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddBatchProcessTask(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("BatchProcessTask")
                .WithDefaultKey()
                .IsAuditable()
                .AddField("TypeId").DataType(DataTypes.Int32).Build
                .AddField("StatusId").DataType(DataTypes.Int32).Build
                .AddField("EntityId").DataType(DataTypes.Int32).Build
                .AddField("EntityTypeId").DataType(DataTypes.Int32).Build
                .AddField("BatchProcessId").DataType(DataTypes.Int32).Build
                .AddField("ResultMessage").MaxLength(128).Build
                .AddField("StartedOn").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("StoppedOn").DataType(DataTypes.DateTimeOffset).Nullable(true).Build

                //.AddRelationship().Relate("", new[] { "Id" }, "", new[] { "Id" }).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddBatchProcessTaskItem(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("BatchProcessTaskItem")
                .WithDefaultKey()
                .IsAuditable()
                .AddField("TaskId").DataType(DataTypes.Int32).Build
                .AddField("StatusId").DataType(DataTypes.Int32).Build
                .AddField("EntityId").DataType(DataTypes.Int32).Build
                .AddField("EntityTypeId").DataType(DataTypes.Int32).Build
                .AddField("Reference").MaxLength(128).Build
                .AddField("ResultMessage").MaxLength(128).Build

                .AddRelationship().Relate("", new[] { "Id" }, "", new[] { "Id" }).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceAction(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceAction")
                .WithDefaultKey()
                .IsAuditable()
                .AddField("TypeId").DataType(DataTypes.Int32).Build
                .AddField("NoticeNumber").MaxLength(128).Build
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("MediaId").DataType(DataTypes.Int32).Build
                .AddField("VehicleId").DataType(DataTypes.Int32).Build
                .AddField("PersonId").DataType(DataTypes.Int32).Build
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("DateIssued").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("NoticeIssuedDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("JuroEntry").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceActionImport(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceActionImport")
                .WithDefaultKey()
                .IsAuditable()
                .AddField("VehicleRegistrationStateId").DataType(DataTypes.Int32).Build
                .AddField("VehicleRegistrationNumber").MaxLength(128).Build
                .AddField("PersonGivenName").MaxLength(128).Build
                .AddField("PersonFamilyName").MaxLength(128).Build
                .AddField("PersonDateOfBirth").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("PersonLicenceNumber").MaxLength(128).Build
                .AddField("PersonLicenceStateId").DataType(DataTypes.Int32).Build
                .AddField("OrganisationAcn").MaxLength(128).Build
                .AddField("OrganisationIncorporatedNumber").MaxLength(128).Build
                .AddField("TypeId").DataType(DataTypes.Int32).Build
                .AddField("DateIssued").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("DateCleared").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("NoticeNumber").MaxLength(128).Build
                .AddField("Line1").MaxLength(128).Build
                .AddField("Line2").MaxLength(128).Build
                .AddField("Line3").MaxLength(128).Build
                .AddField("Line4").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceActionLocation(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceActionLocation")
                .WithDefaultKey()
                .IsAuditable()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("LocalGovernmentArea").MaxLength(128).Build
                .AddField("LgashortTitle").MaxLength(128).Build
                .AddField("Near").MaxLength(128).Build
                .AddField("CommonName").MaxLength(128).Build
                .AddField("Remarks").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .AddField("MapDetail").DataType(DataTypes.Object).DataTypeTypeName("MapDetail").Build

                //.AddField("Altitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("AltitudeAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("HorizontalAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Longitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Latitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Elevation").DataType(DataTypes.Decimal).Nullable(true).Build

                .AddField("Address").DataType(DataTypes.Object).DataTypeTypeName("AddressBook").Build

                //.AddField("FloorNumber").MaxLength(20).Nullable(true).Build
                //.AddField("UnitType").MaxLength(200).Nullable(true).Build
                //.AddField("UnitNumber").MaxLength(20).Nullable(true).Build
                //.AddField("StreetName").MaxLength(128).Nullable(true).Build
                //.AddField("StreetType").MaxLength(128).Nullable(true).Build
                //.AddField("StreetSuffix").MaxLength(200).Nullable(true).Build
                //.AddField("StreetNumberFirst").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberFirstPrefix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberFirstSuffix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLast").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLastPrefix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLastSuffix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetDirectional").MaxLength(200).Nullable(true).Build
                //.AddField("PostalContainer").MaxLength(255).Nullable(true).Build
                //.AddField("PostalCode").MaxLength(20).Nullable(true).Build
                //.AddField("Suburb").MaxLength(100).Nullable(true).Build
                //.AddField("Country").MaxLength(128).Nullable(true).Build
                //.AddField("FullAddress").MaxLength(1500).Nullable(true).Build

                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Nullable(true).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceActionOffence(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceActionOffence")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("Category").MaxLength(128).Build
                .AddField("EffectiveDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("ExpiryDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("ShortTitle").MaxLength(128).Build
                .AddField("Title").MaxLength(128).Build
                .AddField("SortOrder").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DemeritPoints").DataType(DataTypes.Int32).Build
                .AddField("SuspensionPeriod").DataType(DataTypes.Int32).Build
                .AddField("Fine").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("ReportingCategory").MaxLength(128).Build
                .AddField("LegislationReference").MaxLength(128).Build
                .AddField("PenaltyDescription").MaxLength(128).Build
                .AddField("InfringementCategory").MaxLength(128).Build
                .AddField("InfringementIssueCategory").MaxLength(128).Build
                .AddField("LongDescription").MaxLength(128).Build
                .AddField("Levy").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("Code").MaxLength(128).Build
                .AddField("SourceManifestId").MaxLength(128).Build
                .AddField("CausePermitFlag").DataType(DataTypes.Bool).Build
                .AddField("CausePermit").MaxLength(128).Build
                .AddField("InspectorDetails").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceActionService(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceActionService")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("RegisteredAddressId").DataType(DataTypes.Int32).Build
                .AddField("AlternateAddressId").DataType(DataTypes.Int32).Build
                .AddField("Email").MaxLength(128).Build
                .AddField("ServiceEmail").DataType(DataTypes.Bool).Build
                .AddField("HasServiceRegisteredAddress").DataType(DataTypes.Bool).Build
                .AddField("HasServiceAlternateAddress").DataType(DataTypes.Bool).Build
                .AddField("ServiceInPerson").DataType(DataTypes.Bool).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceActionServiceAddress(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceActionServiceAddress")
                .WithDefaultKey()
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("LocalGovernmentArea").MaxLength(128).Build
                .AddField("LgashortTitle").MaxLength(128).Build
                .AddField("Near").MaxLength(128).Build
                .AddField("CommonName").MaxLength(128).Build
                .AddField("Remarks").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("MapDetail").DataType(DataTypes.Object).DataTypeTypeName("MapDetail").Build

                //.AddField("Altitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("AltitudeAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("HorizontalAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Longitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Latitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Elevation").DataType(DataTypes.Decimal).Nullable(true).Build

                .AddField("Address").DataType(DataTypes.Object).DataTypeTypeName("AddressBook").Build

                //.AddField("FloorNumber").MaxLength(20).Nullable(true).Build
                //.AddField("UnitType").MaxLength(200).Nullable(true).Build
                //.AddField("UnitNumber").MaxLength(20).Nullable(true).Build
                //.AddField("StreetName").MaxLength(128).Nullable(true).Build
                //.AddField("StreetType").MaxLength(128).Nullable(true).Build
                //.AddField("StreetSuffix").MaxLength(200).Nullable(true).Build
                //.AddField("StreetNumberFirst").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberFirstPrefix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberFirstSuffix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLast").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLastPrefix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLastSuffix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetDirectional").MaxLength(200).Nullable(true).Build
                //.AddField("PostalContainer").MaxLength(255).Nullable(true).Build
                //.AddField("PostalCode").MaxLength(20).Nullable(true).Build
                //.AddField("Suburb").MaxLength(100).Nullable(true).Build
                //.AddField("Country").MaxLength(128).Nullable(true).Build
                //.AddField("FullAddress").MaxLength(1500).Nullable(true).Build
                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Nullable(true).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceActionStatus(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceActionStatus")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("StatusId").DataType(DataTypes.Int32).Build
                .AddField("Reason").MaxLength(128).Build
                .AddField("RecordedDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("ToProceedOffence").MaxLength(128).Build
                .AddField("GivenName").MaxLength(128).Build
                .AddField("FamilyName").MaxLength(128).Build
                .AddField("UserId").DataType(DataTypes.Int32).Build
                .AddField("HomeLocation").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build
                .AddRelationship().Relate("ComplianceAction", new[] { "Id" }, "ComplianceActionStatus", new[] { "ComplianceActionId" }, RelationshipTypes.One, RelationshipTypes.One).Build
                .AddRelationship().Relate("User", new[] { "Id" }, "ComplianceActionStatus", new[] { "UserId" }, RelationshipTypes.One, RelationshipTypes.One).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceActionSubComponent(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceActionSubComponent")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("Name").MaxLength(128).Build
                .AddField("NameCode").MaxLength(128).Build
                .AddField("Action").MaxLength(128).Build
                .AddField("ActionCode").MaxLength(128).Build
                .AddField("Information").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddRelationship().Relate("ComplianceAction", new[] { "Id" }, "ComplianceActionSubComponent", new[] { "ComplianceActionId" }, RelationshipTypes.One, RelationshipTypes.One).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceDefect(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceDefect")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("Odometer").DataType(DataTypes.Decimal).Precision(1).Scale(18).Build
                .AddField("VehicleUnattended").DataType(DataTypes.Bool).Build
                .AddField("NumberPlateDefective").DataType(DataTypes.Bool).Build
                .AddField("LabelAttached").DataType(DataTypes.Bool).Build
                .AddField("RoadWorthyRequired").DataType(DataTypes.Bool).Build
                .AddField("ConditionOfUse").MaxLength(128).Build
                .AddField("NoticeToBeCleared").MaxLength(128).Build
                .AddField("VehicleNotUsed").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("VehicleNotUsedDays").DataType(DataTypes.Int32).Build
                .AddField("VehicleNotUsedHours").DataType(DataTypes.Int32).Build
                .AddField("InspectionType").MaxLength(128).Build
                .AddField("NatureOfInspection").MaxLength(128).Build
                .AddField("InspectionTypeFull").DataType(DataTypes.Bool).Build
                .AddField("InspectionTypePartial").DataType(DataTypes.Bool).Build
                .AddField("InspectionTypeOther").DataType(DataTypes.Bool).Build
                .AddField("InspectionTypeAudit").DataType(DataTypes.Bool).Build
                .AddField("DefectCategoryMinor").DataType(DataTypes.Bool).Build
                .AddField("DefectCategoryMajor").DataType(DataTypes.Bool).Build
                .AddField("DefectCategoryMajorGrounded").DataType(DataTypes.Bool).Build
                .AddField("DefectCategorySelfClearing").DataType(DataTypes.Bool).Build
                .AddField("VicRoadsInspection").DataType(DataTypes.Bool).Build
                .AddField("InfringementIssued").DataType(DataTypes.Bool).Build
                .AddField("HowIsTheVehicleDefective").MaxLength(128).Build
                .AddField("ClearanceInspection").MaxLength(128).Build
                .AddField("ClearedAt").MaxLength(128).Build
                .AddField("ClearanceOther").MaxLength(128).Build
                .AddField("ClearedReferenceId").MaxLength(128).Build
                .AddField("Directions").MaxLength(128).Build
                .AddField("MethodOfRemoval").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build
                .AddRelationship().Relate("ComplianceAction", new[] { "Id" }, "ComplianceDefect", new[] { "ComplianceActionId" }, RelationshipTypes.One, RelationshipTypes.One).Build
                .AddRelationship().Relate("ComplianceDefect", new[] { "Id" }, "ComplianceDefectSubComponent", new[] { "ComplianceDefectId" }).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceDefectSubComponent(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceDefectSubComponent")
                .WithDefaultKey()
                .AddField("ComplianceDefectId").DataType(DataTypes.Int32).Build
                .AddField("Name").MaxLength(128).Build
                .AddField("NameCode").MaxLength(128).Build
                .AddField("Action").MaxLength(128).Build
                .AddField("ActionCode").MaxLength(128).Build
                .AddField("Information").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build
                .AddRelationship().Relate("ComplianceDefect", new[] { "Id" }, "ComplianceDefectSubComponent", new[] { "ComplianceDefectId" }).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceDirectionDimension(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceDirectionDimension")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("PersonDriverId").DataType(DataTypes.Int32).Build
                .AddField("PersonOperatorId").DataType(DataTypes.Int32).Build
                .AddField("OrganisationOperatorId").DataType(DataTypes.Int32).Build
                .AddField("MassLimit").DataType(DataTypes.Bool).Build
                .AddField("DimensionLimit").DataType(DataTypes.Bool).Build
                .AddField("Unsafe").DataType(DataTypes.Bool).Build
                .AddField("NotSecured").DataType(DataTypes.Bool).Build
                .AddField("Severity").MaxLength(128).Build
                .AddField("SecureLoad").DataType(DataTypes.Bool).Build
                .AddField("ReduceGvm").DataType(DataTypes.Bool).Build
                .AddField("ReduceGvmnumber").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("AdjustAxleGroup").DataType(DataTypes.Bool).Build
                .AddField("AdjustAxleGroupNumber").DataType(DataTypes.Int32).Build
                .AddField("AdjustAxleGroupBy").DataType(DataTypes.Int32).Build
                .AddField("AdjustVehicleDimension").DataType(DataTypes.Bool).Build
                .AddField("AdjustVehicleDimensionLength").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("AdjustVehicleDimensionWidth").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("AdjustVehicleDimensionHeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("AdjustVehicleDimensionOverhang").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("NominatedPlace").MaxLength(128).Build
                .AddField("Conditions").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceDirectionDimensionsHardcopy(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceDirectionDimensionsHardcopy")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("PersonDriverId").DataType(DataTypes.Int32).Build
                .AddField("PersonOperatorId").DataType(DataTypes.Int32).Build
                .AddField("OrganisationOperatorId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceDirectionFatigue(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceDirectionFatigue")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("WorkDiaryPageNumber").MaxLength(128).Build
                .AddField("ReasonableBeliefFatigue").DataType(DataTypes.Bool).Build
                .AddField("ReasonableBeliefMaximumWorkTime").DataType(DataTypes.Bool).Build
                .AddField("ReasonableBeliefMinimumRestTime").DataType(DataTypes.Bool).Build
                .AddField("ReasonableBeliefNoWorkDiary").DataType(DataTypes.Bool).Build
                .AddField("SuitableRestPlace").DataType(DataTypes.Bool).Build
                .AddField("SuitableRestPlaceSuburb").MaxLength(128).Build
                .AddField("SuitableRestPlaceOther").MaxLength(128).Build
                .AddField("WorkReduced").DataType(DataTypes.Bool).Build
                .AddField("WorkReducedHours").DataType(DataTypes.Int32).Build
                .AddField("WorkReducedMinutes").DataType(DataTypes.Int32).Build
                .AddField("ContinuousRest").DataType(DataTypes.Bool).Build
                .AddField("ContinuousRestFrom").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("ContinuousRestTo").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("RestOrBreak").DataType(DataTypes.Bool).Build
                .AddField("RestOrBreakHours").DataType(DataTypes.Int32).Build
                .AddField("RestOrBreakMinutes").DataType(DataTypes.Int32).Build
                .AddField("RestOrBreakCommencing").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceDirectionToRectify(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceDirectionToRectify")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("DescriptionOfBreach").MaxLength(128).Build
                .AddField("DirectedTo").MaxLength(128).Build
                .AddField("DirectedToLocation").MaxLength(128).Build
                .AddField("MovementCondition").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceEducation(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceEducation")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("Category").MaxLength(128).Build
                .AddField("OnRoad").DataType(DataTypes.Bool).Build
                .AddField("TopicOfInterest").MaxLength(128).Build
                .AddField("TimeTakenHours").DataType(DataTypes.Int32).Build
                .AddField("TimeTakenMinutes").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceFormalCaution(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceFormalCaution")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("DetectedSpeed").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("AllegedSpeed").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("PermittedSpeed").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("Information").MaxLength(128).Build
                .AddField("ActionInvolvement").MaxLength(128).Build
                .AddField("Employer").MaxLength(128).Build
                .AddField("Occupation").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceFormalWarning(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceFormalWarning")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("UseTheVehicle").DataType(DataTypes.Bool).Build
                .AddField("PermittedTheUse").DataType(DataTypes.Bool).Build
                .AddField("Other").DataType(DataTypes.Bool).Build
                .AddField("Description").MaxLength(128).Build
                .AddField("ActionInvolvement").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceInfringement(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceInfringement")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("DetectedSpeed").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("AllegedSpeed").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("PermittedSpeed").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("Information").MaxLength(128).Build
                .AddField("ActionInvolvement").MaxLength(128).Build
                .AddField("Employer").MaxLength(128).Build
                .AddField("Occupation").MaxLength(128).Build
                .AddField("AssociateWeighingNotice").DataType(DataTypes.Bool).Build
                .AddField("VehicleOccupants").DataType(DataTypes.Int32).Build
                .AddField("Pplates").DataType(DataTypes.Bool).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceOfficialWarning(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceOfficialWarning")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("UseTheVehicle").DataType(DataTypes.Bool).Build
                .AddField("PermittedTheUse").DataType(DataTypes.Bool).Build
                .AddField("Other").DataType(DataTypes.Bool).Build
                .AddField("Description").MaxLength(128).Build
                .AddField("ActionInvolvement").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceReportForProsecution(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceReportForProsecution")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("ReferenceNumber").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddComplianceWAFormalCaution(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ComplianceWAFormalCaution")
                .WithDefaultKey()
                .AddField("ComplianceActionId").DataType(DataTypes.Int32).Build
                .AddField("UseTheVehicle").DataType(DataTypes.Bool).Build
                .AddField("PermittedTheUse").DataType(DataTypes.Bool).Build
                .AddField("Other").DataType(DataTypes.Bool).Build
                .AddField("Description").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEvent(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Event")
                .WithDefaultKey()
                .IsRoot()
                .SupportCrud(CRUDSupport.Create | CRUDSupport.Read)
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

        private static Fluent.ModuleBuilder AddMapDetail(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("MapDetail")
                .AddField("Altitude").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("AltitudeAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("HorizontalAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("Longitude").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("Latitude").DataType(DataTypes.Decimal).Nullable(true).Build
                .AddField("Elevation").DataType(DataTypes.Decimal).Nullable(true).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddEventAddress(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventAddress")
                .WithDefaultKey()
                .AddField("StateId").DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("EventAddressType").DataType(Domain.DataTypes.Object).DataTypeTypeName("AddressTypes").Nullable(true).Build
                .AddField("LocalGovernmentArea").MaxLength(255).Nullable(true).Build
                .AddField("LGAShortTitle").MaxLength(50).Nullable(true).Build
                .AddField("Near").MaxLength(255).Nullable(true).Build
                .AddField("CommonName").MaxLength(255).Nullable(true).Build
                .AddField("Remarks").MaxLength(255).Nullable(true).Build
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build

                .AddField("MapDetail").DataType(DataTypes.Object).DataTypeTypeName("MapDetail").Build

                //.AddField("Altitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("AltitudeAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("HorizontalAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Longitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Latitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Elevation").DataType(DataTypes.Decimal).Nullable(true).Build

                .AddField("Address").DataType(DataTypes.Object).DataTypeTypeName("AddressBook").Build

                //.AddField("FloorNumber").MaxLength(20).Nullable(true).Build
                //.AddField("UnitType").MaxLength(200).Nullable(true).Build
                //.AddField("UnitNumber").MaxLength(20).Nullable(true).Build
                //.AddField("StreetName").MaxLength(128).Nullable(true).Build
                //.AddField("StreetType").MaxLength(128).Nullable(true).Build
                //.AddField("StreetSuffix").MaxLength(200).Nullable(true).Build
                //.AddField("StreetNumberFirst").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberFirstPrefix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberFirstSuffix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLast").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLastPrefix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLastSuffix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetDirectional").MaxLength(200).Nullable(true).Build
                //.AddField("PostalContainer").MaxLength(255).Nullable(true).Build
                //.AddField("PostalCode").MaxLength(20).Nullable(true).Build
                //.AddField("Suburb").MaxLength(100).Nullable(true).Build
                //.AddField("Country").MaxLength(128).Nullable(true).Build
                //.AddField("FullAddress").MaxLength(1500).Nullable(true).Build

                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Build

                .AddRelationship()
                    .Relate("Event", new[] { "Id" }, "EventAddress", new[] { "EventId" }).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddEventInspection(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventInspection")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("VehicleStandardsInspected").DataType(DataTypes.Bool).Build
                .AddField("VehicleStandardsPassed").DataType(DataTypes.Bool).Build
                .AddField("VehicleStandardsLevel").DataType(DataTypes.Int32).Build
                .AddField("VehicleStandardsEquipment").MaxLength(128).Build
                .AddField("PermitInspected").DataType(DataTypes.Bool).Build
                .AddField("Permits").MaxLength(128).Build
                .AddField("MassInspected").DataType(DataTypes.Bool).Build
                .AddField("MassGenerateWeighingNotice").DataType(DataTypes.Bool).Build
                .AddField("MassWeighingNoticeMediaRef").MaxLength(128).Build
                .AddField("MassWeighingNoticeNumber").MaxLength(128).Build
                .AddField("MassAxleConfiguration").MaxLength(128).Build
                .AddField("MassWeighingCategory").MaxLength(128).Build
                .AddField("MassWeighingMethod").MaxLength(128).Build
                .AddField("MassBridgeCalibratedZeroed").DataType(DataTypes.Bool).Build
                .AddField("MassConformingSite").DataType(DataTypes.Bool).Build
                .AddField("MassDriverViewedWeighing").DataType(DataTypes.Bool).Build
                .AddField("MassOperatorCourseOfTrade").MaxLength(128).Build
                .AddField("MassDriverStatusToOwner").MaxLength(128).Build
                .AddField("MassDriverInstructedBy").MaxLength(128).Build
                .AddField("MassPublicTrade").MaxLength(128).Build
                .AddField("MassGrossEndAndEnd").MaxLength(128).Build
                .AddField("MassWitnessName").MaxLength(128).Build
                .AddField("MassWitnessPhoneNumber").MaxLength(128).Build
                .AddField("MassComments").MaxLength(128).Build
                .AddField("WeighingDataStepCount").DataType(DataTypes.Int32).Build
                .AddField("WeighingDataExtremeAxleDistance").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataCombinedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataDefaultAllowedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataAllowedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataMassAdjustment").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataMassAdjustmentPerStep").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataPercentageOfAllowed").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataPermits").MaxLength(128).Build
                .AddField("WeighBridgeName").MaxLength(128).Build
                .AddField("WeighBridgeExpiry").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("WeighBridgeConfigureDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("WeighBridgeLocation").MaxLength(128).Build
                .AddField("DimensionsInspected").DataType(DataTypes.Bool).Build
                .AddField("DimensionsPassed").DataType(DataTypes.Bool).Build
                .AddField("DimensionsHeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DimensionsWidth").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DimensionsLength").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DimensionsRearOverhang").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DimensionsMeasuringInstrument").MaxLength(128).Build
                .AddField("DimensionsProjections").MaxLength(128).Build
                .AddField("DimensionsSingleVehicle").MaxLength(128).Build
                .AddField("LoadRestraintInspected").DataType(DataTypes.Bool).Build
                .AddField("LoadRestraintPassed").DataType(DataTypes.Bool).Build
                .AddField("LoadRestraintNotes").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventInspectionAxleGroup(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventInspectionAxleGroup")
                .WithDefaultKey()
                .AddField("EventInspectionId").DataType(DataTypes.Int32).Build
                .AddField("DefaultMassAdjustment").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("MassAdjustment").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WheelCount").DataType(DataTypes.Int32).Build
                .AddField("CombinedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("PercentageOfAllowed").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("AllowedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DefaultAllowedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventInspectionAxleWeight(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventInspectionAxleWeight")
                .WithDefaultKey()
                .AddField("EventInspectionAxleGroupId").DataType(DataTypes.Int32).Build
                .AddField("AxleOffset").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("OffSide").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("NearSide").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("CombinedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DistanceBetweenAxles").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("IsCustom").DataType(DataTypes.Bool).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventInspectionAxleWeightPwd(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventInspectionAxleWeightPwd")
                .WithDefaultKey()
                .AddField("EventInspectionAxleWeightId").DataType(DataTypes.Int32).Build
                .AddField("Number").MaxLength(128).Build
                .AddField("Expiry").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventInspectionMovement(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventInspectionMovement")
                .WithDefaultKey()
                .AddField("EventInspectionId").DataType(DataTypes.Int32).Build
                .AddField("FoundWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("PercentageOfAllowed").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("MassAdjustment").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("AllowedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventMediaExternal(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventMediaExternal")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("Type").MaxLength(128).Build
                .AddField("Uri").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventMedium(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventMedium")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("ReferenceId").DataType(DataTypes.UniqueIdentifier).Build
                .AddField("Description").MaxLength(128).Build
                .AddField("MimeType").MaxLength(128).Build
                .AddField("FileName").MaxLength(128).Build
                .AddField("InPerson").DataType(DataTypes.Bool).Build
                .AddField("ThumbnailReferenceId").DataType(DataTypes.UniqueIdentifier).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventNote(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventNote")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("TextValue").MaxLength(128).Build
                .AddField("RecordedDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventOfficer(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventOfficer")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("UserId").DataType(DataTypes.Int32).Build
                .AddField("EventOfficerTypeId").DataType(DataTypes.Int32).Build
                .AddField("HomeStation").MaxLength(128).Build
                .AddField("AuthorityNumber").MaxLength(128).Build
                .AddField("FirstName").MaxLength(128).Build
                .AddField("Surname").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventOrganisation(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventOrganisation")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("EventOrganisationTypeId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventPerson(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventPerson")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("PersonId").DataType(DataTypes.Int32).Build
                .AddField("EventPersonTypeId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventPersonDiary(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventPersonDiary")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("PersonId").DataType(DataTypes.Int32).Build
                .AddField("Presented").DataType(DataTypes.Bool).Build
                .AddField("OtherReason").MaxLength(128).Build
                .AddField("Other").DataType(DataTypes.Bool).Build
                .AddField("LessThan100km").DataType(DataTypes.Bool).Build
                .AddField("LessThan160km").DataType(DataTypes.Bool).Build
                .AddField("LessThan12ton").DataType(DataTypes.Bool).Build
                .AddField("Type").MaxLength(128).Build
                .AddField("BookNumber").MaxLength(128).Build
                .AddField("PageNumber").MaxLength(128).Build
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("IssueDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("ElectronicId").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventPersonFatigue(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventPersonFatigue")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("PersonId").DataType(DataTypes.Int32).Build
                .AddField("Regulated").MaxLength(128).Build
                .AddField("AppearFatigued").DataType(DataTypes.Bool).Build
                .AddField("DriverHomeLocation").MaxLength(128).Build
                .AddField("RecordKeeperName").MaxLength(128).Build
                .AddField("RecordKeeperLocation").MaxLength(128).Build
                .AddField("AccreditationNumber").MaxLength(128).Build
                .AddField("Scheduler").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventVehicle(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventVehicle")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("VehicleId").DataType(DataTypes.Int32).Build
                .AddField("EventVehicleTypeId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventVehicleConcession(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventVehicleConcession")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("VehicleId").DataType(DataTypes.Int32).Build
                .AddField("Code").MaxLength(128).Build
                .AddField("Description").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventVehicleCondition(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventVehicleCondition")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("VehicleId").DataType(DataTypes.Int32).Build
                .AddField("Type").MaxLength(128).Build
                .AddField("Description").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddEventVehicleScheme(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("EventVehicleScheme")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("VehicleId").DataType(DataTypes.Int32).Build
                .AddField("Module").MaxLength(128).Build
                .AddField("Number").MaxLength(128).Build
                .AddField("ExpiryDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddIntercept(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Intercept")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("InterceptNumber").MaxLength(128).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddInterceptImport(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("InterceptImport")
                .WithDefaultKey()
                .AddField("VehicleRegistrationStateId").DataType(DataTypes.Int32).Build
                .AddField("VehicleRegistrationNumber").MaxLength(128).Build
                .AddField("PersonGivenName").MaxLength(128).Build
                .AddField("PersonFamilyName").MaxLength(128).Build
                .AddField("PersonDateOfBirth").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("PersonLicenceNumber").MaxLength(128).Build
                .AddField("PersonLicenceStateId").DataType(DataTypes.Int32).Build
                .AddField("OrganisationAcn").MaxLength(128).Build
                .AddField("OrganisationIncorporatedNumber").MaxLength(128).Build
                .AddField("InterceptNumber").MaxLength(128).Build
                .AddField("DateIssued").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("VehicleInfo").MaxLength(128).Build
                .AddField("NoticesDescription").MaxLength(128).Build
                .AddField("HasNotices").DataType(DataTypes.Bool).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddInterceptJourney(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("InterceptJourney")
                .WithDefaultKey()
                .AddField("InterceptId").DataType(DataTypes.Int32).Build
                .AddField("IndustrySector").MaxLength(128).Build
                .AddField("StartingLocation").MaxLength(128).Build
                .AddField("EndLocation").MaxLength(128).Build
                .AddField("Consignor").MaxLength(128).Build
                .AddField("ConsignorLocation").MaxLength(128).Build
                .AddField("Loader").MaxLength(128).Build
                .AddField("LoaderLocation").MaxLength(128).Build
                .AddField("Consignee").MaxLength(128).Build
                .AddField("ConsigneeLocation").MaxLength(128).Build
                .AddField("PhotosProvided").DataType(DataTypes.Bool).Build
                .AddField("DriverEmployer").MaxLength(128).Build
                .AddField("LoadedGoods").MaxLength(128).Build
                .AddField("LoadedOther").MaxLength(128).Build
                .AddField("Odometer").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddInterceptOrganisation(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("InterceptOrganisation")
                .WithDefaultKey()
                .AddField("InterceptId").DataType(DataTypes.Int32).Build
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddInterceptPerson(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("InterceptPerson")
                .WithDefaultKey()
                .AddField("InterceptId").DataType(DataTypes.Int32).Build
                .AddField("PersonId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddInterceptVehicle(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("InterceptVehicle")
                .WithDefaultKey()
                .AddField("InterceptId").DataType(DataTypes.Int32).Build
                .AddField("VehicleId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddLog(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Log")
                .WithDefaultKey().IsAuditable(false)
                .AddField("LogTypeId").DataType(DataTypes.Int32).Build
                .AddField("LogLevelTypeId").DataType(DataTypes.Int32).Build
                .AddField("LogComponentTypeId").DataType(DataTypes.Int32).Build
                .AddField("UserId").DataType(DataTypes.Int32).Build
                .AddField("SignInName").MaxLength(128).Build
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("Source").MaxLength(128).Build
                .AddField("EventDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("DeviceId").MaxLength(128).Build
                .AddField("Message").MaxLength(128).Build
                .AddField("Exception").MaxLength(128).Build
                .AddField("RequestId").MaxLength(128).Build
                .AddField("RequestBody").MaxLength(128).Build
                .AddField("ResponseBody").MaxLength(128).Build
                .AddField("Duration").DataType(DataTypes.Int32).Build
                .AddField("ClientVersion").MaxLength(128).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddNoticeNumber(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("NoticeNumber")
                .WithDefaultKey()
                .IsRoot()
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("NoticeTypeId").DataType(DataTypes.Int32).Build
                .AddField("Description").MaxLength(128).Build
                .AddField("StartSequenceNumber").DataType(DataTypes.Int32).Build
                .AddField("LastSequenceNumber").DataType(DataTypes.Int32).Build
                .AddField("NumberFormat").MaxLength(128).Build
                .AddField("PerCallAllocation").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddOrganisation(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Organisation")
                .WithDefaultKey()
                .IsRoot()
                .AddField("CreationId").DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("Name").MaxLength(255).Build
                .AddField("ACN").MaxLength(255).Nullable(true).Build
                .AddField("TypeId").DataType(Domain.DataTypes.Object).DataTypeTypeName("OrganisationTypes").Build
                //.AddField("NevdisId").MaxLength(255).Nullable(true).Build
                //.AddField("VicCustomerNo").MaxLength(255).Nullable(true).Build
                //.AddField("ParentId").DataType(DataTypes.Int32).Nullable(true).Build
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build
                .AddField("SystemId").DataType(DataTypes.Int32).Default("30").Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddOrganisationAddress(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("OrganisationAddress")
                .WithDefaultKey()
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("AddressTypeId").DataType(DataTypes.Int32).Build
                .AddField("LocalGovernmentArea").MaxLength(128).Build
                .AddField("LgashortTitle").MaxLength(128).Build
                .AddField("Near").MaxLength(128).Build
                .AddField("CommonName").MaxLength(128).Build
                .AddField("Remarks").MaxLength(128).Build
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("Source").MaxLength(128).Build
                .AddField("MapDetail").DataType(DataTypes.Object).DataTypeTypeName("MapDetail").Build

                //.AddField("Altitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("AltitudeAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("HorizontalAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Longitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Latitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Elevation").DataType(DataTypes.Decimal).Nullable(true).Build

                .AddField("Address").DataType(DataTypes.Object).DataTypeTypeName("AddressBook").Build

                //.AddField("FloorNumber").MaxLength(20).Nullable(true).Build
                //.AddField("UnitType").MaxLength(200).Nullable(true).Build
                //.AddField("UnitNumber").MaxLength(20).Nullable(true).Build
                //.AddField("StreetName").MaxLength(128).Nullable(true).Build
                //.AddField("StreetType").MaxLength(128).Nullable(true).Build
                //.AddField("StreetSuffix").MaxLength(200).Nullable(true).Build
                //.AddField("StreetNumberFirst").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberFirstPrefix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberFirstSuffix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLast").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLastPrefix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLastSuffix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetDirectional").MaxLength(200).Nullable(true).Build
                //.AddField("PostalContainer").MaxLength(255).Nullable(true).Build
                //.AddField("PostalCode").MaxLength(20).Nullable(true).Build
                //.AddField("Suburb").MaxLength(100).Nullable(true).Build
                //.AddField("Country").MaxLength(128).Nullable(true).Build
                //.AddField("FullAddress").MaxLength(1500).Nullable(true).Build

                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Nullable(true).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddOrganisationContact(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("OrganisationContact")
                .WithDefaultKey()
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("ContactTypeId").DataType(DataTypes.Int32).Build
                .AddField("ContactValue").MaxLength(128).Build
                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
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
                .AddField("DateOfRegistration").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("AlertLevel").DataType(DataTypes.Int32).Build
                .AddField("AssociatedAlertLevel").DataType(DataTypes.Int32).Build
                .AddField("Source").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddPerson(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Person")
                .WithDefaultKey()
                .IsRoot()
                .AddField("CreationId").DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("GivenName").MaxLength(255).Build
                .AddField("FamilyName").MaxLength(255).Build
                .AddField("DateOfBirth").DataType(DataTypes.DateTimeOffset).Build
                .AddField("LicenceNumber").MaxLength(255).Nullable(true).Build
                .AddField("LicenceState").DataType(Domain.DataTypes.Object).DataTypeTypeName("States").Build
                .AddField("SystemId").DataType(DataTypes.Int32).Default("30").Build
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build
                .AddField("SourcePersonId").MaxLength(64).Nullable(true).Build
                //.AddField("NevdisId").MaxLength(255).Nullable(true).Build
                //.AddField("ParentId").DataType(DataTypes.Int32).Nullable(true).Build
                .Build;
        }

        private static Fluent.ModuleBuilder AddPersonAddress(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("PersonAddress")
                .WithDefaultKey()
                .AddField("PersonId").DataType(DataTypes.Int32).Build
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("AddressTypeId").DataType(DataTypes.Int32).Build
                .AddField("LocalGovernmentArea").MaxLength(128).Build
                .AddField("LgashortTitle").MaxLength(128).Build
                .AddField("Near").MaxLength(128).Build
                .AddField("CommonName").MaxLength(128).Build
                .AddField("Remarks").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("Source").MaxLength(128).Build
                .AddField("MapDetail").DataType(DataTypes.Object).DataTypeTypeName("MapDetail").Build

                //.AddField("Altitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("AltitudeAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("HorizontalAccuracy").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Longitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Latitude").DataType(DataTypes.Decimal).Nullable(true).Build
                //.AddField("Elevation").DataType(DataTypes.Decimal).Nullable(true).Build

                .AddField("Address").DataType(DataTypes.Object).DataTypeTypeName("AddressBook").Build

                //.AddField("FloorNumber").MaxLength(20).Nullable(true).Build
                //.AddField("UnitType").MaxLength(200).Nullable(true).Build
                //.AddField("UnitNumber").MaxLength(20).Nullable(true).Build
                //.AddField("StreetName").MaxLength(128).Nullable(true).Build
                //.AddField("StreetType").MaxLength(128).Nullable(true).Build
                //.AddField("StreetSuffix").MaxLength(200).Nullable(true).Build
                //.AddField("StreetNumberFirst").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberFirstPrefix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberFirstSuffix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLast").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLastPrefix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetNumberLastSuffix").MaxLength(50).Nullable(true).Build
                //.AddField("StreetDirectional").MaxLength(200).Nullable(true).Build
                //.AddField("PostalContainer").MaxLength(255).Nullable(true).Build
                //.AddField("PostalCode").MaxLength(20).Nullable(true).Build
                //.AddField("Suburb").MaxLength(100).Nullable(true).Build
                //.AddField("Country").MaxLength(128).Nullable(true).Build
                //.AddField("FullAddress").MaxLength(1500).Nullable(true).Build

                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Nullable(true).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddPersonContact(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("PersonContact")
                .WithDefaultKey()
                .AddField("PersonId").DataType(DataTypes.Int32).Build
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("ContactTypeId").DataType(DataTypes.Int32).Build
                .AddField("ContactValue").MaxLength(128).Build
                .AddField("DateAdded").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("Source").MaxLength(128).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddPersonDetail(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("PersonDetail")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("PersonId").DataType(DataTypes.Int32).Build
                .AddField("GivenName").MaxLength(128).Build
                .AddField("FamilyName").MaxLength(128).Build
                .AddField("DateOfBirth").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("MiddleNames").MaxLength(128).Build
                .AddField("Gender").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("AlertLevel").DataType(DataTypes.Int32).Build
                .AddField("AssociatedAlertLevel").DataType(DataTypes.Int32).Build
                .AddField("Source").MaxLength(128).Build
                .AddField("LicenceType").MaxLength(128).Build
                .AddField("LicenceNumber").MaxLength(128).Build
                .AddField("LicenceClasses").MaxLength(128).Build
                .AddField("LicenceConditions").MaxLength(128).Build
                .AddField("LicenceProduced").DataType(DataTypes.Bool).Build
                .AddField("LicenceEffective").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("LicenceExpiry").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("LicenceStatus").MaxLength(128).Build
                .AddField("LicenceStateId").DataType(DataTypes.Int32).Build
                .AddField("InternationalLicence").DataType(DataTypes.Bool).Build
                .AddField("Jurisdiction").MaxLength(128).Build
                .AddField("ProfileImageUrl").MaxLength(128).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddPostcode(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Postcode")
                .WithDefaultKey()
                .AddField("SourceId").DataType(DataTypes.Int32).Build
                .AddField("Postcode1").MaxLength(128).Build
                .AddField("Locality").MaxLength(128).Build
                .AddField("State").MaxLength(128).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddReferenceData(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ReferenceData")
                .WithDefaultKey()
                .AddField("CategoryId").DataType(DataTypes.Int32).Build
                .AddField("Code").MaxLength(128).Build
                .AddField("Name").MaxLength(128).Build
                .AddField("Description").MaxLength(128).Build
                .AddField("EffectFrom").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("EffectTo").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("SortOrder").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddReferenceDataAttribute(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ReferenceDataAttribute")
                .WithDefaultKey()
                .AddField("ReferenceDataId").DataType(DataTypes.Int32).Build
                .AddField("Name").MaxLength(128).Build
                .AddField("Value").MaxLength(128).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddReferenceDataCategory(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ReferenceDataCategory")
                .WithDefaultKey()
                .AddField("Code").MaxLength(128).Build
                .AddField("Name").MaxLength(128).Build
                .AddField("Description").MaxLength(128).Build
                .AddField("SortOrder").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build
                .AddField("SystemUse").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddReferenceDataMapping(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ReferenceDataMapping")
                .WithDefaultKey()
                .AddField("ReferenceDataId").DataType(DataTypes.Int32).Build
                .AddField("ReferenceDataCategoryId").DataType(DataTypes.Int32).Build
                .AddField("SourceValue").MaxLength(128).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddShift(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Shift")
                .WithDefaultKey()
                .IsRoot()
                .AddField("StartTime").DataType(DataTypes.Int32).Build
                .AddField("EndTime").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("EstimatedEndTime").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("Operation").MaxLength(128).Build
                .AddField("Remarks").MaxLength(128).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddShiftOfficer(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("ShiftOfficer")
                .WithDefaultKey()
                .AddField("ShiftId").DataType(DataTypes.Int32).Build
                .AddField("UserId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddUser(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("User")
                .WithDefaultKey()
                .IsRoot()
                .AddField("SignInName").MaxLength(128).Build
                .AddField("FirstName").MaxLength(128).Build
                .AddField("Surname").MaxLength(128).Build
                .AddField("UserOrganisation").DataType(DataTypes.Int32).Build
                .AddField("PrimaryRole").MaxLength(128).Build
                .AddField("ActingRole").MaxLength(128).Build
                .AddField("ActingFrom").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("ActingTo").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("IsAdmin").DataType(DataTypes.Bool).Build
                .AddField("ManageBasicManifest").DataType(DataTypes.Bool).Build
                .AddField("ManageAdvancedManifest").DataType(DataTypes.Bool).Build
                .AddField("EffectFrom").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("EffectTo").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("AuthorityNumber").MaxLength(128).Build
                .AddField("HomeStation").MaxLength(128).Build
                .AddField("Region").MaxLength(128).Build
                .AddField("Team").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddVehicle(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("Vehicle")
                .WithDefaultKey()
                .IsRoot()
                .AddField("CreationId").BusinessKey(true).DataType(Domain.DataTypes.UniqueIdentifier).Nullable(true).Build
                .AddField("RegistrationState").DataType(Domain.DataTypes.Object).DataTypeTypeName("States").Nullable(true).Build
                .AddField("RegistrationNumber").MaxLength(10).Nullable(true).Build
                .AddField("IsNationalRegistration").DataType(DataTypes.Bool).Build
                .AddField("VehicleIdentificationNumber").MaxLength(20).Nullable(true).Build
                .AddField("EngineNumber").MaxLength(20).Nullable(true).Build
                .AddField("ChassisNumber").MaxLength(32).Nullable(true).Build
                .AddField("SourceVehicleId").MaxLength(64).Build
                .AddField("SystemId").DataType(Domain.DataTypes.Object).DataTypeTypeName("SourceSystemTypes").Build
                .AddField("Active").DataType(DataTypes.Bool).Default("true").Build

                .AddBehaviour("Create", BehaviourVerb.Post)
                    .Raising("Created")
                    .AddRequest("VehicleCreateRequest")
                        .AddField("RegistrationState").DataType(Domain.DataTypes.Object).DataTypeTypeName("States").Nullable(true).Build
                        .AddField("RegistrationNumber").MaxLength(10).Nullable(true).Build
                        .AddField("IsNationalRegistration").DataType(DataTypes.Bool).Build
                        .AddField("VehicleIdentificationNumber").MaxLength(20).Nullable(true).Build
                        .AddField("EngineNumber").MaxLength(20).Nullable(true).Build
                        .AddField("ChassisNumber").MaxLength(32).Nullable(true).Build
                        .AddField("SourceVehicleId").MaxLength(64).Build
                        .AddField("SystemId").DataType(Domain.DataTypes.Object).DataTypeTypeName("SourceSystemTypes").Build
                    .Build
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

        private static Fluent.ModuleBuilder AddVehicleFootprint(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("VehicleFootprint")
                .WithDefaultKey()
                .AddField("Latitude").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("Longitude").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("Direction").MaxLength(128).Build
                .AddField("SampleTakenOn").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("VehicleId").DataType(DataTypes.Int32).Build
                .AddField("SourceType").MaxLength(128).Build
                .AddField("SourceReference").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build
                .AddField("StateId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddVehicleFootprintImport(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("VehicleFootprintImport")
                .WithDefaultKey()
                .AddField("Latitude").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("Longitude").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("Direction").MaxLength(128).Build
                .AddField("SampleTakenOn").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("StateId").DataType(DataTypes.Int32).Build
                .AddField("FullAddress").MaxLength(128).Build
                .AddField("VehicleRegistrationStateId").DataType(DataTypes.Int32).Build
                .AddField("VehicleRegistrationNumber").MaxLength(128).Build
                .AddField("SourceType").MaxLength(128).Build
                .AddField("SourceReference").MaxLength(128).Build
                .AddField("SourceRecordId").MaxLength(128).Build
                .AddField("Active").DataType(DataTypes.Bool).Build
                .AddField("SystemId").DataType(DataTypes.Int32).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddVehicleOrganisation(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("VehicleOrganisation")
                .WithDefaultKey()
                .AddField("OrganisationId").DataType(DataTypes.Int32).Build
                .AddField("VehicleId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddVehiclePerson(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("VehiclePerson")
                .WithDefaultKey()
                .AddField("PersonId").DataType(DataTypes.Int32).Build
                .AddField("VehicleId").DataType(DataTypes.Int32).Build
                .AddField("Active").DataType(DataTypes.Bool).Build

                .Build;
        }

        private static Fluent.ModuleBuilder AddWAEventInspection(this Fluent.ModuleBuilder mb)
        {
            return mb
                .AddModel("WAEventInspection")
                .WithDefaultKey()
                .AddField("EventId").DataType(DataTypes.Int32).Build
                .AddField("WainterceptId").DataType(DataTypes.Int32).Build
                .AddField("DangerousGoodsInspected").DataType(DataTypes.Bool).Build
                .AddField("DangerousGoodsHardCopy").DataType(DataTypes.Bool).Build
                .AddField("DangerousGoodsJuroEntry").MaxLength(128).Build
                .AddField("DangerousGoodsClassOrDivisionOfDg").MaxLength(128).Build
                .AddField("DangerousGoodsProduct").MaxLength(128).Build
                .AddField("DangerousGoodsLoadDescriptors").MaxLength(128).Build
                .AddField("DangerousGoodsPrimeContractorName").MaxLength(128).Build
                .AddField("DangerousGoodsPrimeContractorAddress").MaxLength(128).Build
                .AddField("DangerousGoodsPrimeContractorPhone").MaxLength(128).Build
                .AddField("DangerousGoodsConsignorName").MaxLength(128).Build
                .AddField("DangerousGoodsConsignorAddress").MaxLength(128).Build
                .AddField("DangerousGoodsConsignorPhone").MaxLength(128).Build
                .AddField("DangerousGoodsInspectionType").MaxLength(128).Build
                .AddField("DangerousGoodsRemediationNoticeIssued").DataType(DataTypes.Bool).Build
                .AddField("DangerousGoodsRemediationNoticeNumber").MaxLength(128).Build
                .AddField("DangerousGoodsPhotosAttached").DataType(DataTypes.Bool).Build
                .AddField("DangerousGoodsComments").MaxLength(128).Build
                .AddField("DangerousGoodsMediaRef").MaxLength(128).Build
                .AddField("VehicleStandardsInspected").DataType(DataTypes.Bool).Build
                .AddField("VehicleStandardsPassed").DataType(DataTypes.Bool).Build
                .AddField("VehicleStandardsEquipment").MaxLength(128).Build
                .AddField("MassInspected").DataType(DataTypes.Bool).Build
                .AddField("MassWeighingNoticeMediaRef").MaxLength(128).Build
                .AddField("MassWeighingNoticeNumber").MaxLength(128).Build
                .AddField("MassAxleConfiguration").MaxLength(128).Build
                .AddField("MassWeighingCategory").MaxLength(128).Build
                .AddField("MassWeighingMethod").MaxLength(128).Build
                .AddField("MassBridgeCalibratedZeroed").DataType(DataTypes.Bool).Build
                .AddField("MassConformingSite").DataType(DataTypes.Bool).Build
                .AddField("MassDriverViewedWeighing").DataType(DataTypes.Bool).Build
                .AddField("MassOperatorCourseOfTrade").MaxLength(128).Build
                .AddField("MassDriverStatusToOwner").MaxLength(128).Build
                .AddField("MassDriverInstructedBy").MaxLength(128).Build
                .AddField("MassPublicTrade").MaxLength(128).Build
                .AddField("MassGrossEndAndEnd").MaxLength(128).Build
                .AddField("MassWitnessName").MaxLength(128).Build
                .AddField("MassWitnessPhoneNumber").MaxLength(128).Build
                .AddField("MassComments").MaxLength(128).Build
                .AddField("WeighingDataStepCount").DataType(DataTypes.Int32).Build
                .AddField("WeighingDataExtremeAxleDistance").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataCombinedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataDefaultAllowedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataAllowedWeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataMassAdjustment").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataMassAdjustmentPerStep").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataPercentageOfAllowed").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("WeighingDataPermits").MaxLength(128).Build
                .AddField("WeighBridgeName").MaxLength(128).Build
                .AddField("WeighBridgeExpiry").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("WeighBridgeConfigureDate").DataType(DataTypes.DateTimeOffset).Nullable(true).Build
                .AddField("WeighBridgeLocation").MaxLength(128).Build
                .AddField("DimensionsInspected").DataType(DataTypes.Bool).Build
                .AddField("DimensionsPassed").DataType(DataTypes.Bool).Build
                .AddField("DimensionsHeight").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DimensionsWidth").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DimensionsLength").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DimensionsRearOverhang").DataType(DataTypes.Decimal).Precision(2).Scale(18).Build
                .AddField("DimensionsProjections").MaxLength(128).Build
                .AddField("DimensionsOtherDimensions").MaxLength(128).Build
                .AddField("LoadRestraintInspected").DataType(DataTypes.Bool).Build
                .AddField("LoadRestraintPassed").DataType(DataTypes.Bool).Build
                .AddField("LoadRestraintNotes").MaxLength(128).Build

                .Build;
        }
    }
}
