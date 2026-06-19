using ShopStock.Domain.Entities.Roles;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopStock.Web.Areas.Admin.ViewModels.User;

public class EditUserViewModel
{
    public int Id { get; set; }

    [Display(Name = "نام")]
    [MaxLength(100, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
    public string? FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [MaxLength(100, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
    public string? LastName { get; set; }

    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
    [MaxLength(100, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
    [EmailAddress(ErrorMessage = "فرمت {0} معتبر نیست.")]
    [MaxLength(256, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "موبایل")]
    [MaxLength(20, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
    public string? Mobile { get; set; }

    [Display(Name = "کد ملی")]
    [MaxLength(20, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد.")]
    public string? NationalCode { get; set; }

    [Display(Name = "رمز عبور جدید")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد.")]
    public string? Password { get; set; }

    [DisplayName("کاربر فعال است ؟")]
    public bool IsActive { get; set; }

    [Display(Name = "تصویر پروفایل")]
    public IFormFile? ProfilePictureFile { get; set; }

    public string? CurrentProfilePictureName { get; set; }

    public bool RemoveProfilePicture { get; set; }

    public IEnumerable<Role> Roles { get; set; } = new List<Role>();
    public List<int> UserSelectedRoles { get; set; } = new();
}