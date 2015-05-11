using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Vilandagro.Core.Entities;

namespace Vilandagro.Infrastructure.EF
{
    public class VilandagroDbContext : DbContext
    {
        private static readonly ILog Log = LogManager.GetLogger<VilandagroDbContext>();

        public VilandagroDbContext()
            : base("VilandagroDatabase")
        {
            Database.Log = Log.Debug;
            Database.SetInitializer<VilandagroDbContext>(null);
        }

        public IDbSet<Category> Categories { get; set; }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<UnitOfPrice> UnitsOfPrice { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}