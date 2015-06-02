using System.Data.Entity.ModelConfiguration;
using Vilandagro.Core.Entities;

namespace Vilandagro.Infrastructure.EF.Mappings
{
    public class UnitOfPriceMap : EntityTypeConfiguration<UnitOfPrice>
    {
        public UnitOfPriceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Description)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("UnitOfPrice");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
