﻿using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations
{
    public class ProductFeatureConfig : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            // BaseEntity properties
            builder.Property<DateTime>("InsertTime");
            builder.Property<bool>("IsRemoved");
            builder.Property<DateTime?>("UpdateTime");
            builder.Property<DateTime?>("RemoveTime");

            builder.HasQueryFilter(e => !EF.Property<bool>(e, "IsRemoved"));

            //Shadow Property Id for Feature
            builder.Property<int>("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .HasAnnotation("Key", 0);
            builder.HasKey("Id");
        }
    }
}
