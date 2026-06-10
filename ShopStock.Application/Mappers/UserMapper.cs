using ShopStock.Application.DTOs.Account;
using ShopStock.Application.Extensions;
using ShopStock.Domain.Entities.Users;

namespace ShopStock.Application.Mappers
{
    public static class UserMapper
    {
        public static User MapToUser(this RegisterDto dto)
        {
            if (dto == null)
                return null;

            return new User
            {
                UserName = dto.UserName.FixUserName(),
                Email = dto.Email.FixEmail(),
                PasswordHash = dto.Password,
                IsActive = true,
                IsEmailActive = false,
            };
        }
    }
}
