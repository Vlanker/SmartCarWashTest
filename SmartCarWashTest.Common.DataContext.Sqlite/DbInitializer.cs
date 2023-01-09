using System;
using System.Linq;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;

namespace SmartCarWashTest.Common.DataContext.Sqlite
{
    public static class DbInitialize
    {
        public static void Initialize(SmartCarWashContext context)
        {
            if (context.Buyers.Any())
            {
                return; // DB has been seeded
            }

            var buyers = new Buyer[]
            {
                new(1, "Buyer 1"),
                new(2, "Buyer 2"),
            };

            context.Buyers.AddRange(buyers);
            context.SaveChanges();

            var salesPoints = new SalesPoint[]
            {
                new(1, "Sales Point 1")
            };

            context.SalesPoints.AddRange(salesPoints);
            context.SaveChanges();

            var product = new Product[]
            {
                new(1, "Product 1", 100),
                new(2, "Product 2", 1000),
            };

            context.Products.AddRange(product);
            context.SaveChanges();

            var providedProducts = new ProvidedProduct[]
            {
                new(1, 1, 100, 1),
                new(2, 1, 100, 1)
            };

            context.ProvidedProducts.AddRange(providedProducts);
            context.SaveChanges();

            var sales = new Sale[]
            {
                new(1, DateTime.Now.Date, DateTime.Now.TimeOfDay, 1, 1, 100),
                new(2, DateTime.Now.Date, DateTime.Now.TimeOfDay, 1, 2, 2100),
                new(3, DateTime.Now.Date, DateTime.Now.TimeOfDay, 1, null, 1000),
            };

            context.Sales.AddRange(sales);
            context.SaveChanges();


            var salesData = new SalesData[]
            {
                new(1, 1, 1, 100, 1),
                new(2, 1, 1, 100, 2),
                new(3, 2, 2, 2000, 2),
                new(4, 2, 1, 1000, 3),
            };
            context.SalesDataSet.AddRange(salesData);
            context.SaveChanges();
        }
    }
}