using ShopStock.Application.DTOs.User;
using ShopStock.Domain.Entities.Users;
using ShopStock.Domain.Enums.User;

namespace ShopStock.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserListDto>> GetUsersAsync();
        Task<DeleteUserDto?> GetUserForDeleteAsync(int userId);
        Task<AdminCreateUserResult> CreateUserAsync(CreateUserDto dto);
        Task<bool> DeleteUserAsync(int userId);
    }
}
