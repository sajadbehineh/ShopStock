using Microsoft.AspNetCore.Mvc;
using ShopStock.Application.Services.Interfaces;
using ShopStock.Application.DTOs.Account;
using ShopStock.Domain.Enums.Account;
using ShopStock.Web.Areas.UserPanel.ViewModels;
using ShopStock.Web.Extensions;

namespace ShopStock.Web.Areas.UserPanel.Controllers
{
    public class ProfileController : UserPanelBaseController
    {
        private readonly IAccountService _accountService;

        public ProfileController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #region Edit Profile

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.GetUserId();
            var profile = _accountService.GetUserProfileAsync(userId).Result;
            var model = new ProfileViewModel
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Mobile = profile.Mobile,
                NationalCode = profile.NationalCode,
                CurrentPicturePath = $"/storage/ProfilePictures/{profile.CurrentProfilePicture}"
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model, IFormFile? profilePicture, bool deleteCurrentPicture)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var dto = new EditProfileDto
                {
                    UserId = User.GetUserId(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Mobile = model.Mobile,
                    NationalCode = model.NationalCode,
                    RemoveCurrentPicture = deleteCurrentPicture,
                    ImageStream = profilePicture?.OpenReadStream()
                };

                var (isSuccess, finalProfilePicture) = await _accountService.EditProfileAsync(dto);

                if (isSuccess)
                {
                    var fullName = $"{model.FirstName} {model.LastName}".Trim();
                    await this.RefreshUserClaimsAsync(fullName, model.Mobile, finalProfilePicture);

                    TempData["EditeProfileSuccess"] = "پروفایل شما با موفقیت ویرایش شد.";
                    return Redirect("/UserPanel");
                }

                ModelState.AddModelError(string.Empty, "خطایی در ویرایش پروفایل رخ داد.");
            }
            catch (InvalidDataException ex) // خطای مربوط به نامعتبر بودن تصویر
            {
                // پیام دقیق از سمت سرویس: "فایل ارسالی یک تصویر معتبر نیست."
                ModelState.AddModelError("ProfilePicture", ex.Message);
            }
            catch (Exception ex)
            {
                // سایر خطاها مثل ابعاد بزرگ یا خطاهای پیش‌بینی نشده
                ModelState.AddModelError("ProfilePicture", ex.Message);
            }


            return View(model);
        }

        #endregion

        #region Change Password

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new ChangePasswordDto()
            {
                UserId = User.GetUserId(),
                CurrentPassword = model.CurrentPassword,
                NewPassword = model.Password
            };

            var result = await _accountService.ChangePasswordAsync(dto);

            if (result == ChangePasswordResult.Success)
            {
                return RedirectToAction("Logout", "Account");
            }

            switch (result)
            {
                case ChangePasswordResult.InvalidCurrentPassword:
                    ModelState.AddModelError(nameof(model.CurrentPassword), "رمز عبور فعلی صحیح نمی باشد.");
                    break;

                case ChangePasswordResult.UserNotFound:
                    ModelState.AddModelError(string.Empty, "کاربر مورد نظر یافت نشد.");
                    break;

                case ChangePasswordResult.SaveFailed:
                    ModelState.AddModelError(string.Empty, "خطایی در ذخیره تغییرات رخ داد. لطفاً دوباره تلاش کنید.");
                    break;

                default:
                    ModelState.AddModelError(string.Empty, "خطای نامشخصی رخ داد.");
                    break;
            }
            return View(model);
        }

        #endregion
    }
}
