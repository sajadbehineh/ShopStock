using Microsoft.EntityFrameworkCore;
using ShopStock.Domain.Entities.Users;
using ShopStock.Domain.Interfaces;
using ShopStock.Infra.Data.Context;

namespace ShopStock.Infra.Data.Repositories
{
    public class UserRepository(EshopDbContext context) : IUserRepository
    {
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task CreateAsync(User user)
        {
            await context.Users.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            //user.UpdatedAt = DateTime.Now;
            context.Users.Update(user);
        }

        public async Task DeleteAsync(User user)
        {
            user.IsDeleted = true;
            user.IsActive = false;
            user.DeletedAt = DateTime.Now;
            await UpdateAsync(user);
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                await DeleteAsync(user);
            }
        }

        public async Task<bool> IsUserNameExistsAsync(string userName)
        {
            return await context.Users.AnyAsync(u => u.UserName == userName);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByActiveCodeAsync(string activeCode)
        {
            if (string.IsNullOrWhiteSpace(activeCode)) return null;

            return await context.Users
                .FirstOrDefaultAsync(u => u.EmailActiveCode == activeCode && !u.IsEmailActive);
        }

        public async Task<User?> GetUserByUserNameOrEmailAsync(string userNameOrEmail)
        {
            if (string.IsNullOrWhiteSpace(userNameOrEmail)) return null;
            return await context.Users
                .FirstOrDefaultAsync(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
