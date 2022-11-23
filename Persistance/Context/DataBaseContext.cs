using Application.Interfaces;
using Domain.Entities.Account;
using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Persistance.Configurations;

namespace Persistance.Context
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {

        }
        // override SaveChangeAsync() to include updatetime,removetime,inserttime,isremoved to entity
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(
                e => e.State == EntityState.Added ||
                e.State == EntityState.Modified ||
                e.State == EntityState.Deleted);
            foreach (var entityEntry in entries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        entityEntry.Property("RemoveTime").CurrentValue = DateTime.Now;
                        entityEntry.Property("IsRemoved").CurrentValue = true;
                        entityEntry.State = EntityState.Modified;
                        break;
                    case EntityState.Modified:
                        entityEntry.Property("UpdateTime").CurrentValue = DateTime.Now;
                        break;
                    case EntityState.Added:
                        entityEntry.Property("InsertTime").CurrentValue = DateTime.Now;
                        entityEntry.Property("IsRemoved").CurrentValue = false;
                        break;
                    default:
                        break;
                }
            }
            return await base.SaveChangesAsync();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserConfig().Configure(modelBuilder.Entity<User>()); // Config for User
            new CategoryConfig().Configure(modelBuilder.Entity<Category>()); // Config for Category
        }
    }
}
