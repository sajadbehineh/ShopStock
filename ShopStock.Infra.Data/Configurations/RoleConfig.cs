using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopStock.Domain.Entities.Roles;

namespace ShopStock.Infra.Data.Configurations
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            #region Key
            builder.HasKey(x => x.Id);
            #endregion

            #region Validation
            builder.Property(x => x.RoleName).IsRequired().HasMaxLength(200);
            #endregion

            #region Relations

            builder
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

            #endregion
        }
    }
}
