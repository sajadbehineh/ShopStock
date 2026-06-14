using ShopStock.Application.DTOs.User;
using ShopStock.Domain.Enums.User;

namespace ShopStock.Application.Contracts
{
    public interface IAdminUserService
    {
        Task<AdminCreateUserResult> CreateUserAsync(AdminCreateUserDto dto);

    }
}
