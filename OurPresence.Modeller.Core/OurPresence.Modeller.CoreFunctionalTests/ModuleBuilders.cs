namespace OurPresence.Modeller.CoreFunctionalTests
{
    public static class ModuleBuilders
    {
        public static Domain.Module CreateModule()
        {
            var mb = Fluent.Module.Create("SuperSafeBank", "Domain");

            return mb
                .AddAccount()
                .Build;
        }

        public static Domain.Module CreateProject()
        {
            var mb = Fluent.Module.Create("SuperSafeBank", "Domain");

            return mb
                .AddAccount()
                .AddCustomer()
                .Build;
        }

        private static Fluent.ModuleBuilder AddAccount(this Fluent.ModuleBuilder mb)
        {
            return mb.AddModel("Account")
                .WithDefaultKey()
                .AddField("OwnerId").BusinessKey(true).DataType(Domain.DataTypes.UniqueIdentifier).Build
                .AddField("Balance").DataType(Domain.DataTypes.Object).DataTypeTypeName("Money").Build
                .AddBehaviour("Withdraw")
                    .Raising("Withdrawal")
                    .AddField("Amount")
                        .DataType(Domain.DataTypes.Object)
                        .DataTypeTypeName("Money")
                        .Build
                    .Build
                .AddBehaviour("Deposit")
                    .Raising("Deposit")
                    .AddField("Amount")
                        .DataType(Domain.DataTypes.Object)
                        .DataTypeTypeName("Money")
                        .Build
                    .Build
                .Build
                .AddRequest("CreateAccount")
                    .AddField("CustomerId").DataType(Domain.DataTypes.UniqueIdentifier).Build
                    .AddField("AccountId").DataType(Domain.DataTypes.UniqueIdentifier).Build
                    .AddField("Currency").DataType(Domain.DataTypes.Object).DataTypeTypeName("Currency").Build
                    .Build
                .AddRequest("Deposit")
                    .AddField("AccountId").DataType(Domain.DataTypes.UniqueIdentifier).Build
                    .AddField("Amount").DataType(Domain.DataTypes.Object).DataTypeTypeName("Money").Build
                    .Build
                .AddRequest("Withdraw")
                    .AddField("AccountId").DataType(Domain.DataTypes.UniqueIdentifier).Build
                    .AddField("Amount").DataType(Domain.DataTypes.Object).DataTypeTypeName("Money").Build                    
                    .Build;
                
        }

        private static Fluent.ModuleBuilder AddCustomer(this Fluent.ModuleBuilder mb)
        {
            return mb.AddModel("Customer")
                .WithDefaultKey()
                .AddField("FirstName").DataType(Domain.DataTypes.String).MaxLength(100).Build
                .AddField("LastName").DataType(Domain.DataTypes.String).MaxLength(100).Build
                .AddField("Email").BusinessKey(true).DataType(Domain.DataTypes.String).MaxLength(256).Build
                .Build
                .AddRequest("CreateCustomer")
                    .AddField("FirstName").DataType(Domain.DataTypes.String).Build
                    .AddField("LastName").DataType(Domain.DataTypes.String).Build
                    .AddField("Email").DataType(Domain.DataTypes.String).Build
                    .Build;
        }
    }
}
