using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;

namespace SmartCarWashTest.Common.DataContext.EntityTypeConfigurations.Sqlite.Configurations
{
    public class ProvidedProductConfigurations :IEntityTypeConfiguration<ProvidedProduct>
    {
        public void Configure(EntityTypeBuilder<ProvidedProduct> builder)
        {
            builder.ToTable("ProvidedProduct");

            builder.HasKey(providedProduct => providedProduct.Id);

            builder.Property(providedProduct => providedProduct.Id);

            builder.Property(providedProduct => providedProduct.ProductId)
                .IsRequired();

            builder.Property(providedProduct => providedProduct.ProductQuantity)
                .IsRequired();
            
            builder.Property(providedProduct => providedProduct.SalesPointId)
                .IsRequired();

            builder.HasOne(providedProduct => providedProduct.SalePoint)
                .WithMany(salesPoint => salesPoint.ProvidedProduct)
                .HasForeignKey(providedProduct => providedProduct.SalesPointId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}