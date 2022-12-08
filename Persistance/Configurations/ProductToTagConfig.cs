using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations
{
    public class ProductToTagConfig : IEntityTypeConfiguration<ProductToTag>
    {
        public void Configure(EntityTypeBuilder<ProductToTag> builder)
        {
            // BaseEntity properties
            builder.Property<DateTime>("InsertTime");
            builder.Property<bool>("IsRemoved");
            builder.Property<DateTime?>("UpdateTime");
            builder.Property<DateTime?>("RemoveTime");

            builder.HasQueryFilter(e => !EF.Property<bool>(e, "IsRemoved"));

            builder.HasKey(e => new { e.ProductId , e.TagId });
        }
    }
}
