using Application.Interfaces;
using Common;
using Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Context
{
    public class DataBaseContext:DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options):base(options)  
        {

        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = (int)BaseRole.Admin, AccessLevel = Enum.GetName(typeof(BaseRole), BaseRole.Admin) });
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = (int)BaseRole.Customer, AccessLevel = Enum.GetName(typeof(BaseRole), BaseRole.Customer) });

            modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsRemoved);
        }
    }
}
