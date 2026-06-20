using ShopStock.Application.DTOs.User;
using ShopStock.Application.Extensions;
using ShopStock.Application.Mappers;
using ShopStock.Application.Security;
using ShopStock.Domain.Enums.User;
using ShopStock.Domain.Interfaces;
using ShopStock.Application.Services.Interfaces;

namespace ShopStock.Application.Services.Implementations
{
    public class UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IImageService imageService,
        IUnitOfWork unitOfWork) : IUserService
    {
        #region Get Users

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

        #endregion

        #region Get User For Edit

        public async Task<EditUserDto?> GetUserForEditAsync(int userId)
        {
            var user = await userRepository.GetUserWithRolesByIdAsync(userId);

            if (user is null)
                return null;

            return user.MapToEditDto();
        }

        #endregion

        #region Get User For Delete

        public async Task<DeleteUserDto?> GetUserForDeleteAsync(int userId)
        {
            var user = await userRepository.GetUserWithRolesByIdAsync(userId);

            if (user == null) return null;

            return new DeleteUserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                ProfilePicture = user.ProfilePicture,
                UserSelectedRolesName = user.UserRoles.Select(ur => ur.Role.RoleName).ToList()
            };
        }

        #endregion

        #region Create User

        public async Task<CreateUserResult> CreateUserAsync(CreateUserDto dto)
        {
            // Normalization
            dto.Email = dto.Email.FixEmail();
            dto.UserName = dto.UserName.FixUserName();

            if (!string.IsNullOrWhiteSpace(dto.Mobile))
                dto.Mobile = dto.Mobile.FixMobile();

            #region Validations

            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.UserName) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return CreateUserResult.InvalidData;
            }

            if (await userRepository.IsEmailExistsAsync(dto.Email))
                return CreateUserResult.EmailDuplicated;

            if (await userRepository.IsUserNameExistsAsync(dto.UserName))
                return CreateUserResult.UserNameDuplicated;

            if (!string.IsNullOrWhiteSpace(dto.Mobile) &&
                await userRepository.IsMobileExistsAsync(dto.Mobile))
            {
                return CreateUserResult.MobileDuplicated;
            }

            if (dto.UserSelectedRoles is null || !dto.UserSelectedRoles.Any())
                return CreateUserResult.InvalidData;

            #endregion

            dto.ProfilePictureName = await SaveImageFileAsync(dto.ImageStream);

            var user = dto.MapToUser();
            user.PasswordHash = dto.Password.HashPassword();

            await using var transaction = await unitOfWork.BeginTransactionAsync();

            try
            {
                await userRepository.CreateAsync(user);

                var userSaved = await userRepository.SaveAsync();

                if (!userSaved)
                {
                    await transaction.RollbackAsync();
                    DeleteProfileImageIfExists(dto.ProfilePictureName);
                    return CreateUserResult.SaveFailed;
                }

                await roleRepository.AddUserToRolesAsync(user.Id, dto.UserSelectedRoles);

                var rolesSaved = await roleRepository.SaveAsync();

                if (!rolesSaved)
                {
                    await transaction.RollbackAsync();
                    DeleteProfileImageIfExists(dto.ProfilePictureName);
                    return CreateUserResult.RoleSaveFailed;
                }

                await transaction.CommitAsync();

                return CreateUserResult.Success;
            }
            catch
            {
                await transaction.RollbackAsync();

                DeleteProfileImageIfExists(dto.ProfilePictureName);

                return CreateUserResult.SaveFailed;
            }
        }


        #endregion

        #region Edit User

        public async Task<EditUserResult> EditUserAsync(EditUserDto dto)
        {
            var user = await userRepository.GetUserWithRolesByIdAsync(dto.Id);
            if (user is null)
                return EditUserResult.UserNotFound;

            dto.UserName = dto.UserName.FixUserName();
            dto.Email = dto.Email.FixEmail();

            if (await userRepository.IsUserNameExistsAsync(dto.UserName, dto.Id))
                return EditUserResult.DuplicateUserName;

            if (await userRepository.IsEmailExistsAsync(dto.Email, dto.Id))
                return EditUserResult.DuplicateEmail;

            if (!string.IsNullOrWhiteSpace(dto.Mobile) &&
                await userRepository.IsMobileExistsAsync(dto.Mobile, dto.Id))
            {
                return EditUserResult.DuplicateMobile;
            }

            var currentProfilePicture = user.ProfilePicture;
            string? newProfilePicture = null;
            bool currentPictureShouldBeDeleted = false;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = dto.Password.HashPassword();

            if (dto.RemoveCurrentPicture)
            {
                if (currentProfilePicture is not null && currentProfilePicture != "NoPhoto.jpg")
                {
                    user.ProfilePicture = "NoPhoto.jpg";
                    currentPictureShouldBeDeleted = true;
                }
            }
            else if (dto.ImageStream is not null)
            {
                newProfilePicture = await imageService.SaveImageAsync(dto.ImageStream, "ProfilePictures", 250, 250);
                user.ProfilePicture = newProfilePicture;

                if (currentProfilePicture is not null && currentProfilePicture is not "NoPhoto.jpg")
                {
                    currentPictureShouldBeDeleted = true;
                }
            }

            dto.MapToUser(user);

            await userRepository.UpdateAsync(user);

            var saveResult = await userRepository.SaveAsync();
            if (!saveResult)
            {
                if (!string.IsNullOrWhiteSpace(newProfilePicture) && newProfilePicture is not "NoPhoto.jpg")
                {
                    imageService.DeleteImage(newProfilePicture, "ProfilePictures");
                }

                return EditUserResult.EditFailed;
            }

            if (currentPictureShouldBeDeleted && currentProfilePicture is not null && currentProfilePicture is not "NoPhoto.jpg")
            {
                imageService.DeleteImage(currentProfilePicture, "ProfilePictures");
            }

            await roleRepository.UpdateUserRolesAsync(user.Id, dto.UserSelectedRoles);
            await roleRepository.SaveAsync();
            return EditUserResult.Success;
        }

        #endregion

        #region Delete User

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            await userRepository.DeleteAsync(user);
            var deleteResult = await userRepository.SaveAsync();
            if (!deleteResult)
            {
                return false;
            }
            return true;
        }

        #endregion

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

        private void DeleteProfileImageIfExists(string? imageName)
        {
            if (!string.IsNullOrWhiteSpace(imageName) &&
                imageName != "NoPhoto.jpg")
            {
                imageService.DeleteImage(imageName, "ProfilePictures");
            }
        }
    }
}
