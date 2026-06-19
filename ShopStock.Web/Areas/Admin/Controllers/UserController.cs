using Microsoft.AspNetCore.Mvc;
using ShopStock.Application.DTOs.User;
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
            if (!ModelState.IsValid)
                return View(model);

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

                UserSelectedRoles = model.UserSelectedRoles
            };

            // Call the service to create the user
            var result = await userService.CreateUserAsync(dto);

            if (result == CreateUserResult.Success)
                return RedirectToAction("Index");

            // Handle Results and add model errors accordingly
            if (result == CreateUserResult.UserNameDuplicated)
                ModelState.AddModelError(nameof(model.UserName), "این نام کاربری قبلاً ثبت شده است.");

            if (result == CreateUserResult.EmailDuplicated)
                ModelState.AddModelError(nameof(model.Email), "این ایمیل قبلاً ثبت شده است.");

            model.Roles = await roleService.GetAllRolesAsync();

            return View(model);
        }
        #endregion


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var user = await _context.Users.FindAsync(id);
            //if (user == null)
            //{
            //    return NotFound();
            //}
            return View();
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,UserName,Email,EmailActiveCode,IsEmailActive,Mobile,MobileActiveCode,NationalCode,PasswordHash,ProfilePicture,IsActive,Id,CreatedAt,UpdatedAt,DeletedAt,IsDeleted")] User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return NotFound();
        //    }

        //    //if (ModelState.IsValid)
        //    //{
        //    //    try
        //    //    {
        //    //        _context.Update(user);
        //    //        await _context.SaveChangesAsync();
        //    //    }
        //    //    catch (DbUpdateConcurrencyException)
        //    //    {
        //    //        if (!UserExists(user.Id))
        //    //        {
        //    //            return NotFound();
        //    //        }
        //    //        else
        //    //        {
        //    //            throw;
        //    //        }
        //    //    }
        //    //    return RedirectToAction(nameof(Index));
        //    //}
        //    return View(user);
        //}

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
