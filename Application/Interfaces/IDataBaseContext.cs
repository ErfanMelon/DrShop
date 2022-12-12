using Domain.Entities.Account;
using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IDataBaseContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        DbSet<ProductFeature> ProductFeatures { get; set; }
        DbSet<ProductTag> ProductTags { get; set; }
        DbSet<ProductToTag> ProductToTags { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Comment_FeedBack> comment_FeedBacks { get; set; }

        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
