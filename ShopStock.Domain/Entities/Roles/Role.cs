using ShopStock.Domain.Entities.Common;
using ShopStock.Domain.Entities.Relations;

namespace ShopStock.Domain.Entities.Roles
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }

        #region Relations
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        #endregion
    }
}
