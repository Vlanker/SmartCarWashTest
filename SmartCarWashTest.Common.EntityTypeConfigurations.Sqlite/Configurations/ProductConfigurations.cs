using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;

namespace SmartCarWashTest.Common.DataContext.EntityTypeConfigurations.Sqlite.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product", "public");

            builder.HasKey(product => product.Id);

            builder.Property(product => product.Id);

            builder.Property(product => product.Name)
                .IsRequired();

            builder.Property(product => product.Price)
                .HasConversion<double>()
                .IsRequired();

            builder.HasMany(product => product.ProvidedProducts)
                .WithOne(providedProduct => providedProduct.Product)
                .HasForeignKey(providedProduct => providedProduct.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(product => product.SalesDataSet)
                .WithOne(salesData => salesData.Product)
                .HasForeignKey(salesData => salesData.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}