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
            return new User
            {
                UserName = dto.UserName.FixUserName(),
                Email = dto.Email.FixEmail(),
                IsActive = true,
                IsEmailActive = false,
                EmailActiveCode = TokenGenerator.GenerateUniqueToken(),
                ProfilePicture = "NoPhoto.jpg",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };
        }

        public static User MapToUser(this CreateUserDto dto)
        {
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

        public static void MapToUser(this EditUserDto dto, User user)
        {
            user.FirstName = dto.FirstName?.Trim();
            user.LastName = dto.LastName?.Trim();
            user.UserName = dto.UserName.FixUserName();
            user.Mobile = dto.Mobile?.Trim();
            user.NationalCode = dto.NationalCode?.Trim();
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            if (user.Email != dto.Email.FixEmail())
            {
                user.Email = dto.Email.FixEmail();
                user.IsEmailActive = false;
                // TODO Send activation email to user email.
            }
        }

        public static EditUserDto MapToEditDto(this User model)
        {
            return new EditUserDto
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Mobile = model.Mobile,
                NationalCode = model.NationalCode,
                IsActive = model.IsActive,
                CurrentProfilePictureName = model.ProfilePicture,
                UserSelectedRoles = model.UserRoles?.Select(ur => ur.RoleId).ToList() ?? new List<int>()
            };
        }
    }
}
