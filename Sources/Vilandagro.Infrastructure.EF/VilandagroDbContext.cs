using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Common.Logging;
using Vilandagro.Core.Entities;
using Vilandagro.Infrastructure.EF.Mappings;

namespace Vilandagro.Infrastructure.EF
{
    public partial class VilandagroDbContext : DbContext
    {
        static VilandagroDbContext()
        {
            Database.SetInitializer<VilandagroDbContext>(null);
        }

        public VilandagroDbContext()
            : base("Name=VilandagroDatabase")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<UnitOfPrice> UnitOfPrices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ProductPriceMap());
            modelBuilder.Configurations.Add(new SpringProductMap());
            modelBuilder.Configurations.Add(new UnitOfPriceMap());

            this.Database.Log = LogManager.GetLogger<VilandagroDbContext>().Debug;

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}