using ShopStock.Application.DTOs.User;
using ShopStock.Application.Extensions;
using ShopStock.Application.Mappers;
using ShopStock.Application.Security;
using ShopStock.Domain.Enums.User;
using ShopStock.Domain.Interfaces;
using ShopStock.Application.Services.Interfaces;
using ShopStock.Domain.Entities.Users;

namespace ShopStock.Application.Services.Implementations
{
    public class UserService(IUserRepository userRepository, IImageService imageService) : IUserService
    {
        public async Task<IEnumerable<UserListDto>> GetUsersAsync()
        {
            var users = await userRepository.GetAllUsersAsync();
            return users.Select(u => new UserListDto()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                Mobile = u.Mobile,
                NationalCode = u.NationalCode,
                ProfilePicture = u.ProfilePicture,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();
        }

        public async Task<DeleteUserDto?> GetUserForDeleteAsync(int userId)
        {
            var user = await userRepository.GetUserWithRolesAsync(userId);

            if (user == null) return null;

            return new DeleteUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                ProfilePicture = user.ProfilePicture,
                Roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList()
            };
        }

        public async Task<AdminCreateUserResult> CreateUserAsync(CreateUserDto dto)
        {
            if (dto == null)
                return AdminCreateUserResult.InvalidData;

            // normalize the email and username
            dto.Email = dto.Email.FixEmail();
            dto.UserName = dto.UserName.FixUserName();

            #region Validations
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.UserName) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return AdminCreateUserResult.InvalidData;
            }

            if (await userRepository.IsEmailExistsAsync(dto.Email))
            {
                return AdminCreateUserResult.EmailDuplicated;
            }

            if (await userRepository.IsUserNameExistsAsync(dto.UserName))
            {
                return AdminCreateUserResult.UserNameDuplicated;
            }

            if (!string.IsNullOrWhiteSpace(dto.Mobile) && await userRepository.IsMobileExistsAsync(dto.Mobile.FixMobile()))
            {
                return AdminCreateUserResult.MobileDuplicated;
            }
            #endregion

            dto.ProfilePictureName = await SaveImageFileAsync(dto.ImageStream);

            var user = dto.MapToUser();

            if (user == null)
                return AdminCreateUserResult.InvalidData;

            user.PasswordHash = dto.Password.HashPassword();

            await userRepository.CreateAsync(user);
            await userRepository.SaveAsync();

            if (dto.UserSelectedRoles != null && dto.UserSelectedRoles.Any())
            {
                await userRepository.AddUserToRolesAsync(user.Id, dto.UserSelectedRoles ?? new List<int>());
                await userRepository.SaveAsync();
            }

            return AdminCreateUserResult.Success;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user= await userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            await userRepository.DeleteAsync(user);
            await userRepository.SaveAsync();
            return true;
        }

        /// <summary>
        /// این متد مسئول مدیریت ذخیره تصویر پروفایل است.
        /// اگر تصویری ارسال نشده باشد، تصویر پیش‌فرض برگردانده می‌شود.
        /// اگر تصویر وجود داشته باشد، با کمک ImageService ذخیره می‌شود
        /// و فقط نام فایل نهایی برگردانده می‌شود.
        /// </summary>
        private async Task<string> SaveImageFileAsync(Stream? imageStream)
        {
            // اگر کاربر هیچ تصویری انتخاب نکرده باشد
            // تصویر پیش‌فرض سیستم ثبت می‌شود
            if (imageStream == null)
                return "NoPhoto.jpg";

            // ذخیره تصویر در پوشه مشخص
            // سایز را هم می‌توانی بسته به نیاز پروژه تغییر دهی
            var fileName = await imageService.SaveImageAsync(imageStream, "ProfilePictures", 250, 250);

            return fileName;
        }
    }
}
