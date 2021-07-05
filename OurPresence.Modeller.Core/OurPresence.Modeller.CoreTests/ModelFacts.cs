// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using Xunit;
using FluentAssertions;
using ApprovalTests;
using ApprovalTests.Reporters;

namespace OurPresence.Modeller.CoreTests
{
    [UseReporter(typeof(DiffReporter))]
    public static class ModelFacts
    {
        [Fact]
        public static void Model_SetsDefaults_WhenCreated()
        {
            var sut = new Model("Test");

            sut.Name.ToString().Should().Be("Test");
            sut.Schema.Should().BeNull();
            sut.Key.Fields.Should().BeEmpty();
            sut.HasAudit.Should().BeFalse();
            sut.Fields.Should().BeEmpty();
            sut.Indexes.Should().BeEmpty();
            sut.Relationships.Should().BeEmpty();
        }

        [Fact]
        public static void Model_IsEntity_IfKeyIsGuidIDOnly()
        {
            var sut = new Model("Test");
            sut.MakeEntity();
            sut.IsEntity().Should().BeTrue();
            sut.Key.Fields.Add(new Field("Another"));
            sut.IsEntity().Should().BeFalse();
        }

        [Fact]
        public static void Model_Serialization()
        {
            var sut = new Model("Test");
            sut.Fields.Add(new Field("Field1"));

            var idx = new Index("UX_Field1");
            idx.Fields.Add(new IndexField("Field1"));
            sut.Indexes.Add(idx);

            var rel = new Relationship()
            {
                PrincipalModel = new Name("Test"),
                PrincipalType = RelationshipTypes.One,
                DependantModel = new Name("OtherModel"),
                DependantType = RelationshipTypes.Many
            };
            rel.PrincipalFields.Add(new Name("Field1"));
            rel.DependantFields.Add(new Name("OtherField"));
            sut.Relationships.Add(rel);

            var json = sut.ToJson();

            Approvals.VerifyJson(json);

            var actual = json.FromJson<Model>();
            actual.Should().BeEquivalentTo(sut);
        }
    }
}
