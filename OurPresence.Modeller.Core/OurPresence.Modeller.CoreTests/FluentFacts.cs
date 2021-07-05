// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using Xunit;
using ApprovalTests;

namespace OurPresence.Modeller.CoreTests
{
    public class FluentFacts
    {
        [Fact]
        public void Fluent_Module_CreateSucceeds()
        {
            var module = Fluent.Module
                .Create("Company","Project")
                .DefaultSchema("dbo")
                .FeatureName("Feature")
                .AddModel("Model")
                    .IsAuditable(true)
                    .Schema("dbo")
                    .WithDefaultKey()
                    .AddField("Field1")
                        .BusinessKey(true)
                        .DataType(DataTypes.String)
                        .Default("Default")
                        .MaxLength(10)
                        .MinLength(5)
                        .Build
                    .AddField("Field2")
                        .DataType(DataTypes.Decimal)
                        .Scale(12)
                        .Precision(2)
                        .Nullable(true)
                        .MinLength(200)
                        .MaxLength(3000)
                        .Build
                    .Build
                .AddEnumeration("Enumeration")
                    .AsFlag()
                    .AddItem("None")
                    .AddItem("One")
                    .AddItem("Two")
                    .AddItem("Three")
                    .Build
                .AddRequest("ChangeModel")
                    .AddField("ModelId")
                        .Build
                        .WithResponse()
                            .AddField("ModelName")
                            .Build
                        .Build
                    .Build
                .Build;

            Approvals.Verify(module.ToJson());
        }
    }
}
