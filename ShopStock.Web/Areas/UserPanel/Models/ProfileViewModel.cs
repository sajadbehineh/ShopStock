using System.ComponentModel.DataAnnotations;

namespace ShopStock.Web.Areas.UserPanel.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string LastName { get; set; }
        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "شماره وارد شده صحیح نیست فرمت مثال 09126700311")]
        public string Mobile { get; set; }

        [Display(Name = "کد ملی")]
        [MaxLength(15, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد.")]
        public string? NationalCode { get; set; }
        [Display(Name = "تصویر پروفایل")]
        public string? ProfilePicture { get; set; }
        public string? CurrentPicturePath { get; set; }

    }
}
