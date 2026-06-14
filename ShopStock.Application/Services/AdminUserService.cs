using ShopStock.Application.Contracts;
using ShopStock.Application.DTOs.User;
using ShopStock.Application.Extensions;
using ShopStock.Application.Mappers;
using ShopStock.Application.Security;
using ShopStock.Domain.Enums.User;
using ShopStock.Domain.Interfaces;

namespace ShopStock.Application.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;

        public AdminUserService(IUserRepository userRepository, IImageService imageService)
        {
            _userRepository = userRepository;
            _imageService = imageService;
        }

        public async Task<AdminCreateUserResult> CreateUserAsync(AdminCreateUserDto dto)
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

            if (await _userRepository.IsEmailExistsAsync(dto.Email))
            {
                return AdminCreateUserResult.EmailDuplicated;
            }

            if (await _userRepository.IsUserNameExistsAsync(dto.UserName))
            {
                return AdminCreateUserResult.UserNameDuplicated;
            }

            if (!string.IsNullOrWhiteSpace(dto.Mobile) && await _userRepository.IsMobileExistsAsync(dto.Mobile.FixMobile()))
            {
                return AdminCreateUserResult.MobileDuplicated;
            }
            #endregion

            dto.ProfilePictureName = await SaveImageFileAsync(dto.ImageStream);

            var user = dto.MapToUser();

            if (user == null)
                return AdminCreateUserResult.InvalidData;

            user.PasswordHash = dto.Password.HashPassword();

            await _userRepository.CreateAsync(user);

            if (dto.UserSelectedRoles != null && dto.UserSelectedRoles.Any())
            {
                await _userRepository.AddUserToRolesAsync(user.Id, dto.UserSelectedRoles ?? new List<int>());
            }

            await _userRepository.SaveAsync();

            return AdminCreateUserResult.Success;
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
            var fileName = await _imageService.SaveImageAsync(imageStream, "ProfilePictures", 250, 250);

            return fileName;
        }
    }
}
