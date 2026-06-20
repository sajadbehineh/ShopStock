using Microsoft.EntityFrameworkCore;
using ShopStock.Domain.Entities.Relations;
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

        public async Task AddUserToRolesAsync(int userId, IEnumerable<int> roleIds)
        {
            foreach (int roleId in roleIds)
            {
                await context.UserRoles.AddAsync(new UserRole() { UserId = userId, RoleId = roleId });
            }
        }

        public async Task UpdateUserRolesAsync(int userId, IEnumerable<int> selectedRoleIds)
        {
            var currentRoles = await context.UserRoles.Where(ur => ur.UserId == userId).ToListAsync();

            context.UserRoles.RemoveRange(currentRoles);


            var newRoles = selectedRoleIds.Select((roleId) => new UserRole
            {
                UserId = userId,
                RoleId = roleId
            });

            context.AddRange(newRoles);
        }

        public async Task DeleteUserRolesAsync(int userId)
        {
            var currentRoles = await context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            context.UserRoles.RemoveRange(currentRoles);
        }
    }
}
