using Microsoft.EntityFrameworkCore;
using ShopStock.Domain.Entities.Relations;
using ShopStock.Domain.Entities.Roles;
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

        #region Roles
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>().ToTable("UserRole");
        }
    }
}
