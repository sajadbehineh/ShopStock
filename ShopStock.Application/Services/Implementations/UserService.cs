using ShopStock.Application.DTOs.User;
using ShopStock.Application.Extensions;
using ShopStock.Application.Mappers;
using ShopStock.Application.Security;
using ShopStock.Domain.Enums.User;
using ShopStock.Domain.Interfaces;
using ShopStock.Application.Services.Interfaces;
using ShopStock.Domain.Results;

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

        public async Task<ServiceResult<CreateUserResult>> CreateUserAsync(CreateUserDto dto)
        {
            var errors = new List<CreateUserResult>();

            // Normalization
            dto.Email = dto.Email.FixEmail();
            dto.UserName = dto.UserName.FixUserName();

            if (!string.IsNullOrWhiteSpace(dto.Mobile))
                dto.Mobile = dto.Mobile.FixMobile();

            #region Validations

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                errors.Add(CreateUserResult.EmailRequired);
            }

            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                errors.Add(CreateUserResult.UserNameRequired);
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                errors.Add(CreateUserResult.PasswordRequired);
            }

            if (await userRepository.IsEmailExistsAsync(dto.Email))
                errors.Add(CreateUserResult.EmailDuplicated);

            if (await userRepository.IsUserNameExistsAsync(dto.UserName))
                errors.Add(CreateUserResult.UserNameDuplicated);

            if (!string.IsNullOrWhiteSpace(dto.Mobile) &&
                await userRepository.IsMobileExistsAsync(dto.Mobile))
            {
                errors.Add(CreateUserResult.MobileDuplicated);
            }

            if (!dto.UserSelectedRoles.Any())
                errors.Add(CreateUserResult.RoleRequired);

            #endregion

            if (dto.ImageStream != null)
            {
                try
                {
                    dto.ProfilePictureName = await SaveImageFileAsync(dto.ImageStream);
                }
                catch (InvalidDataException)
                {
                    errors.Add(CreateUserResult.InvalidImage);
                }
            }

            if (errors.Any())
                return ServiceResult<CreateUserResult>.Failure(errors.ToArray());

            var user = dto.MapToUser();
            user.PasswordHash = dto.Password.HashPassword();

            #region Start Transaction
            await unitOfWork.BeginTransactionAsync();

            try
            {
                await userRepository.CreateAsync(user);
                await unitOfWork.SaveChangesAsync();

                await roleRepository.AddUserToRolesAsync(user.Id, dto.UserSelectedRoles!);

                await unitOfWork.CommitAsync();

                return ServiceResult<CreateUserResult>.Success();

            }
            catch
            {
                await unitOfWork.RollbackAsync();

                DeleteProfileImageIfExists(dto.ProfilePictureName);

                return ServiceResult<CreateUserResult>.Failure(CreateUserResult.SaveFailed);
            }
            #endregion
        }

        #endregion

        #region Edit User

        public async Task<ServiceResult<EditUserResult>> EditUserAsync(EditUserDto dto)
        {
            var user = await userRepository.GetUserWithRolesByIdAsync(dto.Id);
            var errors = new List<EditUserResult>();

            if (user is null)
                errors.Add(EditUserResult.UserNotFound);

            dto.UserName = dto.UserName.FixUserName();
            dto.Email = dto.Email.FixEmail();

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                errors.Add(EditUserResult.EmailRequired);
            }

            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                errors.Add(EditUserResult.UserNameRequired);
            }


            if (await userRepository.IsUserNameExistsAsync(dto.UserName, dto.Id))
                errors.Add(EditUserResult.UserNameDuplicated);

            if (await userRepository.IsEmailExistsAsync(dto.Email, dto.Id))
                errors.Add(EditUserResult.EmailDuplicated);

            if (!string.IsNullOrWhiteSpace(dto.Mobile) &&
                await userRepository.IsMobileExistsAsync(dto.Mobile, dto.Id))
            {
                errors.Add(EditUserResult.MobileDuplicated);
            }

            if (!dto.UserSelectedRoles.Any())
                errors.Add(EditUserResult.RoleRequired);

            var currentProfilePicture = user!.ProfilePicture;
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
                try
                {
                    newProfilePicture = await SaveImageFileAsync(dto.ImageStream);
                    user.ProfilePicture = newProfilePicture;

                    if (currentProfilePicture is not null && currentProfilePicture is not "NoPhoto.jpg")
                        currentPictureShouldBeDeleted = true;
                }
                catch (InvalidDataException)
                {
                    errors.Add(EditUserResult.InvalidImage);
                }
            }

            if (errors.Any())
                return ServiceResult<EditUserResult>.Failure(errors.ToArray());

            dto.MapToUser(user);

            #region Start Transaction
            await unitOfWork.BeginTransactionAsync();

            try
            {
                await userRepository.UpdateAsync(user);

                await roleRepository.UpdateUserRolesAsync(user.Id, dto.UserSelectedRoles);

                await unitOfWork.CommitAsync();

                if (currentPictureShouldBeDeleted && currentProfilePicture is not null && currentProfilePicture is not "NoPhoto.jpg")
                {
                    imageService.DeleteImage(currentProfilePicture, "ProfilePictures");
                }

                return ServiceResult<EditUserResult>.Success();
            }
            catch
            {
                await unitOfWork.RollbackAsync();

                if (!string.IsNullOrWhiteSpace(newProfilePicture) && newProfilePicture is not "NoPhoto.jpg")
                {
                    imageService.DeleteImage(newProfilePicture, "ProfilePictures");
                }

                return ServiceResult<EditUserResult>.Failure(EditUserResult.EditFailed);
            }
            #endregion
        }

        #endregion

        #region Delete User

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            await userRepository.DeleteAsync(user);
            var deleteResult = await unitOfWork.SaveChangesAsync();

            return deleteResult > 0;
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
            if (imageStream is null)
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
