using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopStock.Domain.Entities.Users;

namespace ShopStock.Infra.Data.Configurations.UserConfig
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            #region Key
            builder.HasKey(u => u.Id);
            #endregion

            #region Validations
            builder.Property(u => u.FirstName).HasMaxLength(200);
            builder.Property(u => u.LastName).HasMaxLength(200);
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.Property(u => u.NationalCode).HasMaxLength(15);
            builder.Property(u => u.EmailActiveCode).HasMaxLength(50);
            builder.Property(u => u.Mobile).HasMaxLength(13);
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(400);

            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique(); 
            builder.HasIndex(u => u.Mobile).IsUnique();

            #endregion

            #region Relations

            #endregion
        }
    }
}
