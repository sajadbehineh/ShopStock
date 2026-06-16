using ShopStock.Application.Services.Interfaces;
using ShopStock.Domain.Entities.Roles;
using ShopStock.Domain.Interfaces;

namespace ShopStock.Application.Services.Implementations
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await roleRepository.GetAllAsync();
        }
    }
}
