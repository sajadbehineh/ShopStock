using ShopStock.Domain.Entities.Roles;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopStock.Web.Areas.Admin.ViewModels.User;

public class CreateUserViewModel
{
    [DisplayName("نام")]
    public string? FirstName { get; set; }

    [DisplayName("نام خانوادگی")]
    public string? LastName { get; set; }

    [DisplayName("نام کاربری")]
    [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
    public string UserName { get; set; }

    [DisplayName("ایمیل")]
    [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
    [EmailAddress(ErrorMessage = "لطفاً یک ایمیل معتبر وارد کنید")]
    public string Email { get; set; }

    [DisplayName("تلفن همراه")]
    [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "شماره وارد شده صحیح نیست فرمت مثال 09120001234")]
    public string? Mobile { get; set; }

    [DisplayName("کد ملی")]
    public string? NationalCode { get; set; }

    [DisplayName("رمز عبور")]
    [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
    //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "کلمه عبور باید حداقل 8 کاراکتر و شامل حروف بزرگ، کوچک، اعداد و کاراکترهای خاص باشد.")]
    public string Password { get; set; }

    //[DisplayName("تصویر پروفایل")]
    public string? ProfilePictureName { get; set; }

    [DisplayName("انتخاب تصویر پروفایل")]
    public IFormFile? ProfilePictureFile { get; set; }

    [DisplayName("کاربر فعال است ؟")]
    public bool IsActive { get; set; }

    public IEnumerable<Role>? Roles { get; set; }
    public List<int>? UserSelectedRoles { get; set; }
}