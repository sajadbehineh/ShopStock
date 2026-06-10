using ShopStock.Application.DTOs.Account;
using ShopStock.Domain.Enums;

namespace ShopStock.Application.Contracts
{
    public interface IAccountService
    {
        Task<RegisterUserResult> RegisterAsync(RegisterDto dto);
        Task<LoginResultDto> LoginAsync(LoginDto dto);
        Task<bool> ActiveAccountAsync(string activeCode);
        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
        Task<bool> IsProfileComplete(int usrId);
        Task<(bool IsSuccess, string ProfilePicture)> EditProfileAsync(EditProfileDto dto);
        Task<EditProfileDto> GetUserProfileAsync(int userId);
    }
}