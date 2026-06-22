using Microsoft.AspNetCore.Mvc;
using ShopStock.Application.DTOs.User;
using ShopStock.Application.Services.Implementations;
using ShopStock.Application.Services.Interfaces;
using ShopStock.Domain.Enums.User;
using ShopStock.Web.Areas.Admin.ViewModels.User;
using ShopStock.Web.Mappers;

namespace ShopStock.Web.Areas.Admin.Controllers
{
    public class UserController(IUserService userService, IRoleService roleService)
        : AdminBaseController
    {
        #region Get Users
        public async Task<IActionResult> Index()
        {
            var users = await userService.GetUsersAsync();

            var model = users.MapToListViewModels();

            return View(model);
        }
        #endregion 

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var user = await _context.Users
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (user == null)
            //{
            //    return NotFound();
            //}

            return View();
        }

        #region Create User
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateUserViewModel()
            {
                Roles = await roleService.GetAllRolesAsync()
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            //if (!ModelState.IsValid)
            //    return View(model);

            // Create a DTO to send to the service layer
            var dto = new CreateUserDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Mobile = model.Mobile,
                NationalCode = model.NationalCode,
                Password = model.Password,
                IsActive = model.IsActive,

                ProfilePictureName = model.ProfilePictureName,
                ImageStream = model.ProfilePictureFile?.OpenReadStream(),

                UserSelectedRoles = model.UserSelectedRoles ?? new List<int>()
            };

            // Call the service to create the user
            var result = await userService.CreateUserAsync(dto);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "اطلاعات کاربر جدید با موفقیت ثبت شد.";
                return RedirectToAction(nameof(Index));
            }

            // Handle Results and add model errors accordingly
            foreach (var error in result.Errors)
            {
                switch (error)
                {
                    case CreateUserResult.UserNameDuplicated:
                        ModelState.AddModelError(nameof(model.UserName), "این نام کاربری قبلاً ثبت شده است.");
                        break;

                    case CreateUserResult.EmailDuplicated:
                        ModelState.AddModelError(nameof(model.Email), "این ایمیل قبلاً ثبت شده است.");
                        break;

                    case CreateUserResult.MobileDuplicated:
                        ModelState.AddModelError(nameof(model.Mobile), "این شماره موبایل قبلاً ثبت شده است.");
                        break;

                    case CreateUserResult.InvalidImage:
                        ModelState.AddModelError(nameof(model.ProfilePictureFile), "*فایل انتخاب شده یک تصویر نامعتبر است.");
                        break;

                    case CreateUserResult.EmailRequired:
                        ModelState.AddModelError(nameof(model.Email), "ایمیل الزامی است");
                        break;

                    case CreateUserResult.UserNameRequired:
                        ModelState.AddModelError(nameof(model.UserName), "نام کاربری الزامی است");
                        break;

                    case CreateUserResult.PasswordRequired:
                        ModelState.AddModelError(nameof(model.Password), "کلمه عبور الزامی است");
                        break;

                    case CreateUserResult.RoleRequired:
                        TempData["RoleRequiredMessage"] = "انتخاب حداقل یک نقش برای کاربر الزامیست.";
                        break;

                    case CreateUserResult.SaveFailed:
                        //ModelState.AddModelError(string.Empty, "ثبت کاربر جدید با خطا مواجه شد.");
                        TempData["FailedMessage"] = "ثبت کاربر جدید با خطا مواجه شد.";
                        break;
                }
            }

            model.Roles = await roleService.GetAllRolesAsync();

            return View(model);
        }
        #endregion

        #region Edit User

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var userDto = await userService.GetUserForEditAsync(id);
            if (userDto == null)
            {
                return NotFound();
            }

            var model = userDto.ToEditViewModel();
            model.Roles = await roleService.GetAllRolesAsync();

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model, bool deleteCurrentPicture)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = await roleService.GetAllRolesAsync();
                return View(model);
            }

            var dto = model.MapToEditDto();
            dto.RemoveCurrentPicture = deleteCurrentPicture;
            var result = await userService.EditUserAsync(dto);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "اطلاعات کاربر با موفقیت ویرایش شد.";
                return RedirectToAction(nameof(Index));
            }

            if (result.Errors.Contains(EditUserResult.UserNotFound))
            {
                TempData["ErrorMessage"] = "کاربر مورد نظر یافت نشد.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                switch (error)
                {
                    case EditUserResult.UserNameDuplicated:
                        ModelState.AddModelError(nameof(model.UserName), "این نام کاربری قبلاً ثبت شده است.");
                        break;

                    case EditUserResult.EmailDuplicated:
                        ModelState.AddModelError(nameof(model.Email), "این ایمیل قبلاً ثبت شده است.");
                        break;

                    case EditUserResult.MobileDuplicated:
                        ModelState.AddModelError(nameof(model.Mobile), "این شماره موبایل قبلاً ثبت شده است.");
                        break;

                    case EditUserResult.InvalidImage:
                        ModelState.AddModelError(nameof(model.ProfilePictureFile), "*فایل انتخاب شده یک تصویر نامعتبر است.");
                        break;

                    case EditUserResult.EmailRequired:
                        ModelState.AddModelError(nameof(model.Email), "ایمیل الزامی است");
                        break;

                    case EditUserResult.UserNameRequired:
                        ModelState.AddModelError(nameof(model.UserName), "نام کاربری الزامی است");
                        break;

                    case EditUserResult.RoleRequired:
                        TempData["RoleRequiredMessage"] = "انتخاب حداقل یک نقش برای کاربر الزامیست.";
                        break;

                    case EditUserResult.EditFailed:
                        //ModelState.AddModelError(string.Empty, "ویرایش کاربر با خطا مواجه شد.");
                        TempData["FailedMessage"] = "ویرایش کاربر با خطا مواجه شد.";
                        break;
                }
            }
            model.Roles = await roleService.GetAllRolesAsync();
            return View(model);
        }

        #endregion

        #region Delete User
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await userService.GetUserForDeleteAsync(id);

            if (dto == null)
                return NotFound();

            var model = dto.MapToDeleteViewModel();

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteUserViewModel model)
        {
            var result = await userService.DeleteUserAsync(model.Id);

            if (!result)
            {
                // TODO نمایش پیغام خطا در صفحه حذف کاربر
                ViewBag.Delete = "عملیات حذف با مشکل مواجه شد. لطفاً مجدد امتحان کنید.";
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
