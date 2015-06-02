using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Vilandagro.Core.Entities;

namespace Vilandagro.Infrastructure.EF.Mappings
{
    public class SpringProductMap : EntityTypeConfiguration<SpringProduct>
    {
        public SpringProductMap()
        {
            // Table & Column Mappings
            this.ToTable("SpringProduct");
            this.Property(t => t.Diametr).HasColumnName("Diametr");
            this.Property(t => t.Weight).HasColumnName("Weight");
        }
    }
}