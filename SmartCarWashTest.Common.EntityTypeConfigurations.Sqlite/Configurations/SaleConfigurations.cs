using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;

namespace SmartCarWashTest.Common.DataContext.EntityTypeConfigurations.Sqlite.Configurations
{
    public class SaleConfigurations : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sale", "public");

            builder.HasKey(sale => sale.Id);

            builder.Property(sale => sale.Id);

            builder.Property(sale => sale.SalesPointId)
                .IsRequired();

            builder.Property(sale => sale.BuyerId);

            builder.Property(sale => sale.Date)
                .IsRequired();

            builder.Property(sale => sale.Time)
                .IsRequired();

            builder.Property(sale => sale.TotalAmount)
                .HasConversion<double>()
                .IsRequired();
        }
    }
}