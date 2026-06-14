using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopStock.Web.Areas.Admin.ViewModels.User
{
    public class UserListViewModel
    {
        public int Id { get; set; }

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
        public string? Mobile { get; set; }

        [DisplayName("کد ملی")]
        public string? NationalCode { get; set; }

        //[DisplayName("کلمه عبور")]
        //[Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        //public string PasswordHash { get; set; }

        [DisplayName("تصویر پروفایل")]
        public string? ProfilePicture { get; set; }

        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
