using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;

namespace SmartCarWashTest.Common.DataContext.EntityTypeConfigurations.Sqlite.Configurations
{
    public class SalesPointConfigurations : IEntityTypeConfiguration<SalesPoint>
    {
        public void Configure(EntityTypeBuilder<SalesPoint> builder)
        {
            builder.ToTable("SalesPoint", "public");

            builder.HasKey(salesPoint => salesPoint.Id);

            builder.Property(salesPoint => salesPoint.Id);

            builder.Property(salesPoint => salesPoint.Name)
                .IsRequired();
        }
    }
}