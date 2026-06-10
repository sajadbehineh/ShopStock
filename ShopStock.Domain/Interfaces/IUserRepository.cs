using ShopStock.Domain.Entities.Users;

namespace ShopStock.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task DeleteAsync(int userId);

        Task<bool> IsUserNameExistsAsync(string userName);
        Task<bool> IsEmailExistsAsync(string email);

        Task<User?> GetUserByActiveCodeAsync(string activeCode);
        Task<User?> GetUserByUserNameOrEmailAsync(string userNameOrEmail);

        Task SaveAsync();
    }
}
