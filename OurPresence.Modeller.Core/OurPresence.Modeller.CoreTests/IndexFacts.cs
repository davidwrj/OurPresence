﻿using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using Xunit;
using FluentAssertions;
using System.ComponentModel;
using ApprovalTests.Reporters;
using ApprovalTests;

namespace OurPresence.Modeller.CoreTests
{
    [UseReporter(typeof(DiffReporter))]
    public static class IndexFacts
    {
        [Fact]
        public static void Index_SetsDefaults_WhenCreated()
        {
            var sut = new Domain.Index("Test");
            sut.Name.ToString().Should().Be("Test");
            sut.IsClustered.Should().BeFalse();
            sut.IsUnique.Should().BeTrue();
        }

        [Fact]
        public static void Index_Serialization()
        {
            var sut = new Domain.Index("Test");
            sut.Fields.Add(new IndexField("Field1") { Sort = ListSortDirection.Descending });
            var json = sut.ToJson();

            Approvals.VerifyJson(json);

            var actual = json.FromJson<Domain.Index>();
            actual.Should().BeEquivalentTo(sut);
        }
    }

}