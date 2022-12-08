using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // BaseEntity properties
            builder.Property<bool>("IsRemoved");
            builder.Property<DateTime?>("UpdateTime");
            builder.Property<DateTime?>("RemoveTime");


            builder.HasQueryFilter(e => !EF.Property<bool>(e, "IsRemoved"));

            builder.HasOne(e => e.Category).WithMany(e => e.Products);
            builder.HasMany(e => e.ProductImages).WithOne().IsRequired(true);
            builder.HasMany(e => e.ProductFeatures).WithOne().IsRequired(true);
            builder.HasMany(e => e.ProductTags).WithOne(e => e.Product).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
