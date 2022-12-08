using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations
{
    public class ProductTagConfig : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            // BaseEntity properties
            builder.Property<DateTime>("InsertTime");
            builder.Property<bool>("IsRemoved");
            builder.Property<DateTime?>("UpdateTime");
            builder.Property<DateTime?>("RemoveTime");

            builder.HasQueryFilter(e => !EF.Property<bool>(e, "IsRemoved"));

            builder.HasKey(e => e.TagId);
            builder.HasIndex(e => e.Tag);
            builder.HasMany(e => e.ProductTags).WithOne(e => e.Tag).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
