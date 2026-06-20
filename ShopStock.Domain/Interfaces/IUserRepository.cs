using ShopStock.Domain.Entities.Users;

namespace ShopStock.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserWithRolesByIdAsync(int userId);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task DeleteAsync(int userId);

        Task<bool> IsUserNameExistsAsync(string userName, int? excludeUserId = null);
        Task<bool> IsEmailExistsAsync(string email, int? excludeUserId = null);
        Task<bool> IsMobileExistsAsync(string mobile, int? excludeUserId = null);

        Task<User?> GetUserByActiveCodeAsync(string activeCode);
        Task<User?> GetUserByUserNameOrEmailAsync(string userNameOrEmail);
    }
}
