using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // BaseEntity properties
            builder.Property<DateTime>("InsertTime");
            builder.Property<bool>("IsRemoved");
            builder.Property<DateTime?>("UpdateTime");
            builder.Property<DateTime?>("RemoveTime");

            builder.HasQueryFilter(e => !EF.Property<bool>(e, "IsRemoved"));
        }
    }
}
