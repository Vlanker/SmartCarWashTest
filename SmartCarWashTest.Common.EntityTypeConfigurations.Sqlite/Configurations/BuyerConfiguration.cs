using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;

namespace SmartCarWashTest.Common.DataContext.EntityTypeConfigurations.Sqlite.Configurations
{
    public class BuyerConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.ToTable("Buyer", "public");

            builder.HasKey(buyer => buyer.Id);

            builder.Property(buyer => buyer.Id);

            builder.Property(buyer => buyer.Name)
                .IsRequired();

            builder.HasMany(buyer => buyer.Sales)
                .WithOne(sale => sale.Buyer)
                .HasForeignKey(sale => sale.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}