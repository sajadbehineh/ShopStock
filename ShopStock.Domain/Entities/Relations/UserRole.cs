using ShopStock.Domain.Entities.Roles;
using ShopStock.Domain.Entities.Users;

namespace ShopStock.Domain.Entities.Relations
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        #region Relations
        public User User { get; set; }
        public Role Role { get; set; }
        #endregion
    }
}
