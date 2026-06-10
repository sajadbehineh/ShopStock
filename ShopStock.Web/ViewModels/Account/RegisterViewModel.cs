using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopStock.Web.ViewModels.Account
{
    public class RegisterViewModel
    {
        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "طول {0} نمی‌تواند از {1} کاراکتر بیشتر باشد.")]
        public string UserName { get; set; }

        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "طول {0} نمی‌تواند از {1} کاراکتر بیشتر باشد.")]
        [EmailAddress(ErrorMessage = "لطفا یک آدرس ایمیل معتبر وارد کنید.")]
        public string Email { get; set; }

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "کلمه عبور باید حداقل 8 کاراکتر و شامل حروف بزرگ، کوچک، اعداد و کاراکترهای خاص باشد.")]
        public string Password { get; set; }

        [DisplayName("تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه عبور و تکرار کلمه عبور با هم مطابقت ندارند.")]
        public string RePassword { get; set; }
    }
}
