// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using Xunit;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;

namespace OurPresence.Modeller.CoreTests
{
    public class ModuleFacts
    {
        [Fact]
        public void Module_Namespace_returnsExpected()
        {
            var sut = new Module("Company", "MasterDatum[MasterData]");

            sut.Namespace.Should().Be("Company.MasterData");
        }

        [Theory]
        [InlineData("company", "projects", null, "Company.Project")]
        [InlineData("company", "projects", "feature", "Company.Project.Feature")]
        public void Module_ReturnsNamespace_FromOtherProperties(string company, string project, string feature, string expected)
        {
            var sut = new Module(company, project)
            {
                Feature = feature == null ? null : new Name(feature)
            };
            sut.Namespace.Should().Be(expected);
        }

        [Fact]
        public void ModuleExtension_FindRelationshipByModel_OnManyToManySucceeds()
        {
            var module = new Module("Company", "Test");
            var model1 = module.AddModel("model1");
            model1.MakeEntity();

            var model2 = module.AddModel("model2");
            model2.MakeEntity();
            model2.AddFieldString("name", 50);

            model1.Relationships.Add(new Relationship
            {
                PrincipalModel = model1.Name,
                PrincipalFields = new List<Name> { new Name("Id") },
                PrincipalType = RelationshipTypes.Many,
                DependantModel = model2.Name,
                DependantFields = new List<Name> { new Name("Id") },
                DependantType = RelationshipTypes.Many,
                LinkTable = model1.Name.Value + model2.Name.Value
            });

            var actual = module.FindRelationshipsByModel(model2);
            actual.Should().HaveCount(1);

            var r = actual.First();
            r.PrincipalModel.Should().Be(model2.Name);
            r.DependantModel.Should().Be(model1.Name);
            r.PrincipalFields.Should().HaveCount(1);
            r.PrincipalFields.First().Value.Should().Be("Id");
            r.DependantFields.Should().HaveCount(1);
            r.DependantFields.First().Value.Should().Be("Id");
            r.LinkTable.Should().Be(model1.Name.Value + model2.Name.Value);
        }

        [Fact]
        public void ModuleExtension_FindRelationshipByModel_Succeeds()
        {
            var module = new Module("Company", "Test");
            var model1 = module.AddModel("model1");
            model1.MakeEntity();

            var model2 = module.AddModel("model2");
            model2.MakeEntity();
            model2.AddFieldString("name", 50);

            module.AddForeignKey(model1, model2);

            var actual = module.FindRelationshipsByModel(model1);
            actual.Should().HaveCount(1);

            var r = actual.First();
            r.PrincipalModel.Should().Be(model1.Name);
            r.DependantModel.Should().Be(model2.Name);
            r.PrincipalFields.Should().HaveCount(1);
            r.PrincipalFields.First().Value.Should().Be("Id");
            r.DependantFields.Should().HaveCount(1);
            r.DependantFields.First().Value.Should().Be("Model1Id");
            r.LinkTable.Should().BeNull();
        }

        [Fact]
        public void ModuleExtension_IsForeignKey_Succeeds()
        {
            var module = new Module("Company", "Test");
            var model1 = module.AddModel("model1");
            model1.MakeEntity();

            var model2 = module.AddModel("model2");
            model2.MakeEntity();
            model2.AddFieldString("name", 50);

            module.AddForeignKey(model1, model2);

            module.IsForeignKey(model2, new[] { "Model1Id" }).Should().BeTrue();
            var field = model2.Fields.Single(f => f.Name.Value == "Model1Id");
            field.IsForeignKey(model2, module).Should().BeTrue();
        }

        [Fact]
        public void ModuleExtension_FindRelationshipByModel_CompositeKeySucceeds()
        {
            var module = new Module("Company", "Test");
            var model1 = module.AddModel("model1");
            model1.Key.Fields.Add(new Field("name"));
            model1.Key.Fields.Add(new Field("tenantId"));

            var model2 = module.AddModel("model2");
            model2.MakeEntity();
            model2.AddFieldString("name", 50);

            module.AddForeignKey(model1, model2);
            model2.Fields.Should().HaveCount(3);

            var actual = module.FindRelationshipsByModel(model1);
            actual.Should().HaveCount(1);

            var r = actual.First();
            r.PrincipalModel.Should().Be(model1.Name);
            r.DependantModel.Should().Be(model2.Name);
            r.PrincipalFields.Should().HaveCount(2);
            r.DependantFields.Should().HaveCount(2);
            r.DependantFields.First().Value.Should().Be("Model1Name");
            r.DependantFields.Last().Value.Should().Be("Model1TenantId");
        }

        [Fact]
        public void ModuleExtension_IsForeignKey_CompositeKeySucceeds()
        {
            var module = new Module("Company", "Test");
            var model1 = module.AddModel("model1");
            model1.Key.Fields.Add(new Field("name"));
            model1.Key.Fields.Add(new Field("tenantId"));

            var model2 = module.AddModel("model2");
            model2.MakeEntity();
            model2.AddFieldString("name", 50);

            module.AddForeignKey(model1, model2);

            module.IsForeignKey(model2, new[] { "Model1Name", "Model1TenantId" }).Should().BeTrue();
            module.IsForeignKey(model2, new[] { "Model1Name" }).Should().BeFalse();
        }
    }
}
