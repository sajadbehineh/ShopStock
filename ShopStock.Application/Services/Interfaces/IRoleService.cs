using ShopStock.Domain.Entities.Roles;

namespace ShopStock.Application.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}
