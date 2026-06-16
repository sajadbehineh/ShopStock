using ShopStock.Domain.Entities.Users;

namespace ShopStock.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserWithRolesAsync(int userId);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task DeleteAsync(int userId);

        Task AddUserToRolesAsync(int userId, IEnumerable<int> roleIds);

        Task<bool> IsUserNameExistsAsync(string userName);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsMobileExistsAsync(string mobile);

        Task<User?> GetUserByActiveCodeAsync(string activeCode);
        Task<User?> GetUserByUserNameOrEmailAsync(string userNameOrEmail);

        Task SaveAsync();
    }
}
