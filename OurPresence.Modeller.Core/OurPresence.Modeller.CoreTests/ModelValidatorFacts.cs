// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using Xunit;
using OurPresence.Modeller.Domain.Validators;
using FluentValidation.TestHelper;
using System.Collections.Generic;

namespace OurPresence.Modeller.CoreTests
{
    public static class ModelValidatorFacts
    {
        [Fact]
        public static void Model_IsNotValid_With2BusinessKeys()
        {
            var model = new Model("Test");

            model.Fields.Add(new Field("Field1") { BusinessKey = true });
            model.Fields.Add(new Field("Field2") { BusinessKey = true });

            var sut = new ModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.Fields, model);
        }

        [Fact]
        public static void Model_IsNotValid_WithAuditFieldsAndHasAudit()
        {
            var model = new Model("Test");

            model.Fields.Add(new Field("Created"));
            model.Fields.Add(new Field("Modified"));
            model.Fields.Add(new Field("CreatedBy"));
            model.Fields.Add(new Field("ModifiedBy"));

            var sut = new ModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.HasAudit, model);
        }

        [Fact]
        public static void Model_IsNotValid_WithDuplicateFields()
        {
            var model = new Model("Test");

            model.Fields.Add(new Field("Field1"));
            model.Fields.Add(new Field("Field1"));

            var sut = new ModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.Fields, model);
        }

        [Fact]
        public static void Model_IsNotValid_WithNoFields()
        {
            var model = new Model("Test");

            var sut = new ModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.Fields, model);
        }

        [Fact]
        public static void Model_IsNotValid_WithNoKeyFields()
        {
            var model = new Model("Test");

            var sut = new ModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.Key.Fields, model);
        }

        [Fact]
        public static void Model_IsNotValid_WithIndexFieldsThatAreNotThere()
        {
            var model = new Model("Test");

            var idx = new Index("FK_Test");
            idx.Fields.Add(new IndexField("IDX_Field"));
            model.Indexes.Add(idx);

            var sut = new ModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.Indexes, model);
        }

        [Fact]
        public static void Model_IsNotValid_WithRelationshipFieldsThatAreNotThere()
        {
            var model = new Model("Test");
            var relationship = new Relationship()
            {
                PrincipalFields = new List<Name>() { new Name("REL_Field1") },
                PrincipalModel = new Name("Test"),
                PrincipalType = RelationshipTypes.One,
                DependantFields = new List<Name>() { new Name("OtherField") },
                DependantModel = new Name("Other"),
                DependantType = RelationshipTypes.Many
            };
            model.Relationships.Add(relationship);

            var sut = new ModelValidator();
            sut.ShouldHaveValidationErrorFor(m => m.Relationships, model);
        }
    }
}
