using Microsoft.EntityFrameworkCore;
using ShopStock.Domain.Entities.Roles;
using ShopStock.Domain.Interfaces;
using ShopStock.Infra.Data.Context;

namespace ShopStock.Infra.Data.Repositories
{
    public class RoleRepository(EshopDbContext context) : IRoleRepository
    {
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await context.Roles.ToListAsync();
        }
    }
}
