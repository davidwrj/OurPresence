// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using Xunit;
using FluentAssertions;

namespace OurPresence.Modeller.CoreTests
{
    public class JsonExtensionFacts
    {
        //[Fact]
        //public static void ToJson_Serialise_WillIgnoreReferenceLoops()
        //{
        //    var settings = new Mock<ISettings>();

        //    var x = settings.Object.ToJson();

        //    x.Should().NotBeNull();
        //}

        [Fact]
        public void Json_Serialization_ProducesEquivalentModule()
        {
            var module = new Module("Jbssa", "SecurityPortal");
            var model = new Model("Application");
            model.MakeEntity();
            model.Fields.Add(new Field("Name") { DataType = DataTypes.String, MaxLength = 100, Nullable = false, BusinessKey = true });
            model.Fields.Add(new Field("Description") { MaxLength = 1000 });
            model.Fields.Add(new Field("IsActive") { DataType = DataTypes.Bool, Default = "true", Nullable = false });
            module.Models.Add(model);

            var index = new Index("PK_ApplicationName") { IsUnique = true };
            index.Fields.Add(new IndexField("Name"));
            model.Indexes.Add(index);
            var relationship = new Relationship()
            {
                PrincipalModel = new Name("Application"),
                PrincipalType = RelationshipTypes.One,
                DependantModel = new Name("Module"),
                DependantType = RelationshipTypes.Many
            };
            relationship.PrincipalFields.Add(new Name("Id"));
            relationship.DependantFields.Add(new Name("ApplicationId"));
            model.Relationships.Add(relationship);

            module
                .ToJson()
                .FromJson<Module>()
                .Should()
                .BeEquivalentTo(module);
        }
    }
}
