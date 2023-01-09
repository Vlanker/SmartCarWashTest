using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;

namespace SmartCarWashTest.Common.DataContext.EntityTypeConfigurations.Sqlite.Configurations
{
    public class SalesDataConfigurations : IEntityTypeConfiguration<SalesData>
    {
        public void Configure(EntityTypeBuilder<SalesData> builder)
        {
            builder.ToTable("SalesData");

            builder.HasKey(salesData => salesData.Id);

            builder.Property(salesData => salesData.Id);

            builder.Property(salesData => salesData.ProductId)
                .IsRequired();

            builder.Property(salesData => salesData.ProductIdAmount)
                .IsRequired();

            builder.Property(salesData => salesData.ProductQuantity)
                .IsRequired();
            
            builder.Property(salesData => salesData.SaleId)
                .IsRequired();
        }
    }
}