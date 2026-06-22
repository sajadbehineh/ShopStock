using ShopStock.Application.DTOs.User;
using ShopStock.Domain.Enums.User;
using ShopStock.Domain.Results;
using ShopStock.Domain.Results.User;

namespace ShopStock.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserListDto>> GetUsersAsync();
        Task<EditUserDto?> GetUserForEditAsync(int userId);
        Task<DeleteUserDto?> GetUserForDeleteAsync(int userId);
        Task<ServiceResult<CreateUserResult>> CreateUserAsync(CreateUserDto dto);
        Task<ServiceResult<EditUserResult>> EditUserAsync(EditUserDto dto);
        Task<bool> DeleteUserAsync(int userId);
    }
}
