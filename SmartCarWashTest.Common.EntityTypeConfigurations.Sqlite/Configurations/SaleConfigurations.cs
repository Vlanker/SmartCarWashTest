using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;

namespace SmartCarWashTest.Common.DataContext.EntityTypeConfigurations.Sqlite.Configurations
{
    public class SaleConfigurations : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sale");

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

            builder.HasOne(sale => sale.SalesPoint)
                .WithMany(salesPoint => salesPoint.Sales)
                .HasForeignKey(sale => sale.SalesPointId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(sale => sale.SalesDataSet)
                .WithOne(salesData => salesData.Sale)
                .HasForeignKey(salesData => salesData.SaleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}