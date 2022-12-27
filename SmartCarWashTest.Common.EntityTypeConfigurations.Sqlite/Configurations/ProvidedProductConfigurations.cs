using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;

namespace SmartCarWashTest.Common.DataContext.EntityTypeConfigurations.Sqlite.Configurations
{
    public class ProvidedProductConfigurations :IEntityTypeConfiguration<ProvidedProduct>
    {
        public void Configure(EntityTypeBuilder<ProvidedProduct> builder)
        {
            builder.ToTable("ProvidedProduct", "public");

            builder.HasKey(providedProduct => providedProduct.Id);

            builder.Property(providedProduct => providedProduct.Id);

            builder.Property(providedProduct => providedProduct.ProductId)
                .IsRequired();

            builder.Property(providedProduct => providedProduct.ProductQuantity)
                .IsRequired();
        }
    }
}