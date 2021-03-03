using Xunit;
using ApprovalTests;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;

namespace OurPresence.Modeller.CoreFunctionalTests
{
    public class FluentFacts
    {
        [Fact]
        public void Fluent_Module_CreateSucceeds()
        {
            var module = Fluent.Module
                .Create("Mizrael", "SuperSafeBank")
                .AddRequest("CreateAccount")
                    .AddField("CustomerId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("Currency").DataType(DataTypes.Object).DataTypeTypeName("Currency").Build                    
                    .Build
                .AddRequest("CreateCustomer")
                    .AddField("FirstName").DataType(DataTypes.String).Build
                    .AddField("LastName").DataType(DataTypes.String).Build
                    .AddField("Email").DataType(DataTypes.String).Build
                    .Build
                .AddRequest("Deposit")
                    .AddField("AccountId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("Amount").DataType(DataTypes.Object).DataTypeTypeName("Money").Build
                    .Build
                .AddRequest("Withdraw")
                    .AddField("AccountId").DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("Amount").DataType(DataTypes.Object).DataTypeTypeName("Money").Build
                    .Build
                .AddModel("Customer")
                    .WithDefaultKey()
                    .AddField("FirstName").MaxLength(100).Build
                    .AddField("LastName").MaxLength(100).Build
                    .AddField("Email").DataType(DataTypes.Object).DataTypeTypeName("Email").BusinessKey(true).MaxLength(100).Build
                    .AddIndex("UK_Email")
                        .AddField("Email").Sort(System.ComponentModel.ListSortDirection.Ascending).Build
                        .Build
                    .Build
                .AddModel("Account")
                    .IsAuditable(true)
                    .WithDefaultKey()
                    .AddField("OwnerId").BusinessKey(true).DataType(DataTypes.UniqueIdentifier).Build
                    .AddField("Balance").DataType(DataTypes.Object).DataTypeTypeName("Money").Build
                    .AddBehaviour("Withdraw").UsingRequest("Withdraw").Raising("Withdrawal")
                        .AddField("Amount").DataType(DataTypes.Object).DataTypeTypeName("Money").Build
                        .Build
                    .AddBehaviour("Deposit").UsingRequest("Deposit").Raising("Deposit")
                        .AddField("Amount").DataType(DataTypes.Object).DataTypeTypeName("Money").Build
                        .Build
                    .AddRelationship().Relate("Customer", new[] {"Id" },"Account", new[] {"OwnerId" }).Build
                    .Build
                .Build;

            Approvals.Verify(module.ToJson());
        }
    }
}
