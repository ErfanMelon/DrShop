using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Configurations
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property<bool>("IsRemoved");
            builder.Property<DateTime?>("UpdateTime");
            builder.Property<DateTime?>("RemoveTime");


            builder.HasQueryFilter(e => !EF.Property<bool>(e, "IsRemoved"));

            builder.HasOne(e => e.Product).WithMany(e => e.Comments);
            builder.HasOne(e => e.User).WithMany(e => e.Comments);
            builder.HasMany(e => e.Advantages).WithOne();
            builder.HasMany(e => e.Disadvantages).WithOne();
        }
    }
    public class Comment_FeedBackConfig : IEntityTypeConfiguration<Comment_FeedBack>
    { 
        public void Configure(EntityTypeBuilder<Comment_FeedBack> builder)
        {
            builder.Property<DateTime>("InsertTime");
            builder.Property<bool>("IsRemoved");
            builder.Property<DateTime?>("UpdateTime");
            builder.Property<DateTime?>("RemoveTime");

            builder.HasQueryFilter(e => !EF.Property<bool>(e, "IsRemoved"));

            builder.Property<int>("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .HasAnnotation("Key", 0);
            builder.HasKey("Id");
        }
    }
}
