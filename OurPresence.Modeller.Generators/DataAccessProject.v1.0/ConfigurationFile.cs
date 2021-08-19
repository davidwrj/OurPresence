// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace EntityFrameworkProject
{
    internal class ConfigurationFile : IGenerator
    {
        private readonly Module _module;

        public ConfigurationFile(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Common.Enums");
            sb.Al("{");
            sb.I(1).Al($"internal static class Configuration");
            sb.I(1).Al("{");
            sb.I(2).Al("public static IServiceCollection AddProductServices(this IServiceCollection services)");
            sb.I(3).Al("=> services");
            sb.I(4).Al(".AddCommandHandler<RegisterProduct, HandleRegisterProduct>(s =>");
            sb.I(4).Al("{");
            sb.I(5).Al("var dbContext = s.GetRequiredService<WarehouseDBContext>();");
            sb.I(5).Al("return new HandleRegisterProduct(dbContext.AddAndSave, dbContext.ProductWithSKUExists);");
            sb.I(4).Al("})");
            sb.I(4).Al(".AddQueryHandler<GetProducts, IReadOnlyList<ProductListItem>, HandleGetProducts>(s =>");
            sb.I(4).Al("{");
            sb.I(5).Al("var dbContext = s.GetRequiredService<WarehouseDBContext>();");
            sb.I(5).Al("return new HandleGetProducts(dbContext.Set<Product>().AsNoTracking());");
            sb.I(4).Al("})");
            sb.I(4).Al(".AddQueryHandler<GetProductDetails, ProductDetails?, HandleGetProductDetails>(s =>");
            sb.I(4).Al("{");
            sb.I(5).Al("var dbContext = s.GetRequiredService<WarehouseDBContext>();");
            sb.I(5).Al("return new HandleGetProductDetails(dbContext.Set<Product>().AsNoTracking());");
            sb.I(4).Al("});");
            sb.I(1).Al("");
            sb.I(1).Al("");
            sb.I(2).Al("public static IEndpointRouteBuilder UseProductsEndpoints(this IEndpointRouteBuilder endpoints) =>");
            sb.I(3).Al("endpoints");
            sb.I(4).Al(".UseRegisterProductEndpoint()");
            sb.I(4).Al(".UseGetProductsEndpoint()");
            sb.I(4).Al(".UseGetProductDetailsEndpoint();");
            sb.I(1).Al("");
            sb.I(2).Al("public static void SetupProductsModel(this ModelBuilder modelBuilder)");
            sb.I(3).Al("=> modelBuilder.Entity<Product>()");
            sb.I(4).Al(".OwnsOne(p => p.Sku);");
            sb.I(1).Al("}");
            sb.Al("}");

            var filename = "Configuration";
            if (Settings.SupportRegen)
            {
                filename += ".generated";
            }
            filename += ".cs";

            return new File(filename, sb.ToString(), canOverwrite: true, path: "Storage");
        }
    }
}
