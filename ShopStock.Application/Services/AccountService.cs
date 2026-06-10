using ShopStock.Application.Contracts;
using ShopStock.Application.DTOs.Account;
using ShopStock.Application.Extensions;
using ShopStock.Application.Generators;
using ShopStock.Application.Mappers;
using ShopStock.Application.Security;
using ShopStock.Domain.Enums;
using ShopStock.Domain.Interfaces;

namespace ShopStock.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;

        public AccountService(IUserRepository userRepository, IImageService imageService)
        {
            _userRepository = userRepository;
            _imageService = imageService;
        }

        #region Register User

        public async Task<RegisterUserResult> RegisterAsync(RegisterDto dto)
        {
            #region Validations
            if (string.IsNullOrWhiteSpace(dto.UserName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
                return RegisterUserResult.InvalidInputs;

            if (await _userRepository.IsUserNameExistsAsync(dto.UserName.FixUserName()))
            {
                return RegisterUserResult.UserNameAlreadyExists;
            }

            if (await _userRepository.IsEmailExistsAsync(dto.Email.FixEmail()))
            {
                return RegisterUserResult.EmailAlreadyExists;
            }
            #endregion

            #region Register

            var user = dto.MapToUser();

            user.PasswordHash = dto.Password.HashPassword();
            user.EmailActiveCode = TokenGenerator.GenerateUniqueToken();
            user.ProfilePicture = "NoPhoto.jpg";
            user.CreatedAt = DateTime.Now;

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveAsync();
            return RegisterUserResult.Success;

            #endregion
        }

        #endregion

        #region Login User

        public async Task<LoginResultDto> LoginAsync(LoginDto dto)
        {
            var result = new LoginResultDto();

            if (string.IsNullOrWhiteSpace(dto.UserNameOrEmail) || string.IsNullOrWhiteSpace(dto.Password))
            {
                result.Status = LoginUserResult.InvalidInputs;
                return result;
            }

            var user = await _userRepository.GetUserByUserNameOrEmailAsync(dto.UserNameOrEmail);
            if (user == null)
            {
                result.Status = LoginUserResult.UserNotFound;
                return result;
            }

            if (user.IsDeleted)
            {
                result.Status = LoginUserResult.UserNotFound;
                return result;
            }

            if (!PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash))
            {
                result.Status = LoginUserResult.InvalidPassword;
                return result;
            }

            if (!user.IsActive)
            {
                result.Status = LoginUserResult.UserNotActive;
                return result;
            }

            if (!user.IsEmailActive)
            {
                result.Status = LoginUserResult.EmailNotActivated;
                return result;
            }

            // لاگین موفقیت آمیز بود؛ اطلاعات کلیم‌ها را پر می‌کنیم
            result.Status = LoginUserResult.Success;
            result.User = new UserClaimsDto()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                ProfilePicture = user.ProfilePicture
            };

            return result;
        }

        #endregion

        #region Make Email Active

        public async Task<bool> ActiveAccountAsync(string activeCode)
        {
            if (string.IsNullOrWhiteSpace(activeCode)) return false;

            var user = await _userRepository.GetUserByActiveCodeAsync(activeCode);
            if (user == null) return false;

            user.IsEmailActive = true;
            user.EmailActiveCode = TokenGenerator.GenerateUniqueToken();

            await _userRepository.SaveAsync();
            return true;
        }

        #endregion

        #region Change Password

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (user == null) throw new Exception("کاربر یافت نشد");

            if (!PasswordHelper.VerifyPassword(dto.OldPassword, user.PasswordHash))
                return false;

            user.PasswordHash = dto.NewPassword.HashPassword();
            await _userRepository.SaveAsync();
            return true;
        }

        #endregion

        #region Check Profile Completeness

        public async Task<bool> IsProfileComplete(int usrId)
        {
            var user = await _userRepository.GetUserByIdAsync(usrId);
            if (user.Mobile != null && user.Mobile != "")
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Edit Profile

        public async Task<(bool IsSuccess, string ProfilePicture)> EditProfileAsync(EditProfileDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (user == null) return (false, "no-image.jpg");

            // 1- Update basic information
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Mobile = dto.Mobile;
            user.NationalCode = dto.NationalCode;

            // 2- Handle profile picture with two scenarios: delete current picture or save new one

            // 2-1. delete current picture if requested
            if (dto.RemoveCurrentPicture)
            {
                if (user.ProfilePicture != "no-image.jpg")
                {
                    _imageService.DeleteImage(user?.ProfilePicture, "ProfilePictures");
                    user.ProfilePicture = "no-image.jpg"; // set to default image
                }
            }

            // 2-2. save new profile picture if provided
            else if (dto.ImageStream != null)
            {
                // delete current picture if it's not the default one
                
                if (user.ProfilePicture != "no-image.jpg")
                {
                    _imageService.DeleteImage(user.ProfilePicture, "ProfilePictures");
                }
                // save new picture
                var newFileName = await _imageService.SaveImageAsync(dto.ImageStream, "ProfilePictures", 250, 250);
                user.ProfilePicture = newFileName;
            }
            // 3- Save changes to database
            await _userRepository.SaveAsync();
            return (true, user.ProfilePicture);
        }

        #endregion

        #region Get User Profile

        public async Task<EditProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            return new EditProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                NationalCode = user.NationalCode,
                CurrentProfilePicture = user.ProfilePicture
            };
        }

        #endregion
    }
}
