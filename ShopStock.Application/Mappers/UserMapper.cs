using ShopStock.Application.DTOs.Account;
using ShopStock.Application.DTOs.User;
using ShopStock.Application.Extensions;
using ShopStock.Application.Generators;
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

        public static User? MapToUser(this AdminCreateUserDto dto)
        {
            if (dto == null)
                return null;

            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName.FixUserName(),
                Email = dto.Email.FixEmail(),
                Mobile = dto.Mobile,
                NationalCode = dto.NationalCode,
                IsActive = dto.IsActive,
                ProfilePicture = dto.ProfilePictureName,
                IsEmailActive = true,
                EmailActiveCode = TokenGenerator.GenerateUniqueToken(),
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };
        }
    }
}
