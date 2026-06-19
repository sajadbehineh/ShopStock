using ShopStock.Application.DTOs.User;
using ShopStock.Domain.Enums.User;

namespace ShopStock.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserListDto>> GetUsersAsync();
        Task<EditUserDto?> GetUserForEditAsync(int userId);
        Task<DeleteUserDto?> GetUserForDeleteAsync(int userId);
        Task<CreateUserResult> CreateUserAsync(CreateUserDto dto);
        Task<EditUserResult> EditUserAsync(EditUserDto dto);
        Task<bool> DeleteUserAsync(int userId);
    }
}
