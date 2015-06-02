using System.Data.Entity.ModelConfiguration;
using Vilandagro.Core.Entities;

namespace Vilandagro.Infrastructure.EF.Mappings
{
    public class ProductPriceMap : EntityTypeConfiguration<ProductPrice>
    {
        public ProductPriceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ProductPrice");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.UnitOfPriceId).HasColumnName("UnitOfPriceId");

            // Relationships
            this.HasRequired(t => t.Product)
                .WithMany(t => t.ProductPrices)
                .HasForeignKey(d => d.ProductId);
            this.HasRequired(t => t.UnitOfPrice)
                .WithMany(t => t.ProductPrices)
                .HasForeignKey(d => d.UnitOfPriceId);

        }
    }
}
