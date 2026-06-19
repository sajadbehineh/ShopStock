using ShopStock.Domain.Entities.Roles;

namespace ShopStock.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task AddUserToRolesAsync(int userId, IEnumerable<int> roleIds);
        Task UpdateUserRolesAsync(int userId, IEnumerable<int> selectedRolesId);
        Task DeleteUserRolesAsync(int userId);
        Task<bool> SaveAsync();
    }
}
