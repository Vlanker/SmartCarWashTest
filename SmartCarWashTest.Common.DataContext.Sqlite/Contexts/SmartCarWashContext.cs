using Microsoft.EntityFrameworkCore;
using SmartCarWashTest.Common.DataContext.EntityTypeConfigurations.Sqlite.Configurations;
using SmartCarWashTest.Common.DataContext.Sqlite.Constants;
using SmartCarWashTest.Common.EntityModels.Sqlite.Entities;

namespace SmartCarWashTest.Common.DataContext.Sqlite.Contexts
{
#pragma warning disable CS1591

    public class SmartCarWashContext : DbContext
    {
        public virtual DbSet<Buyer> Buyers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProvidedProduct> ProvidedProducts { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SalesData> SalesDataSet { get; set; }
        public virtual DbSet<SalesPoint> SalesPoints { get; set; }

        protected SmartCarWashContext()
        {
        }

        public SmartCarWashContext(DbContextOptions<SmartCarWashContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder.UseSqlite(ProjectConstants.DefaultConnection,
                builder => builder.MigrationsAssembly(ProjectConstants.MigrationProject));

            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BuyerConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfigurations());
            modelBuilder.ApplyConfiguration(new ProvidedProductConfigurations());
            modelBuilder.ApplyConfiguration(new SaleConfigurations());
            modelBuilder.ApplyConfiguration(new SalesDataConfigurations());
            modelBuilder.ApplyConfiguration(new SalesPointConfigurations());
        }
    }

#pragma warning restore CS1591
}