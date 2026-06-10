using Microsoft.EntityFrameworkCore;
using ShopStock.Domain.Entities.Users;

namespace ShopStock.Infra.Data.Context
{
    public class EshopDbContext(DbContextOptions<EshopDbContext> options)
        : DbContext(options)
    {
        #region Users
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
