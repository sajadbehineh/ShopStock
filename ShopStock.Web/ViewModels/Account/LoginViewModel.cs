using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopStock.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        [DisplayName("نام کاربری یا ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string UserNameOrEmail { get; set; }

        [DisplayName("کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "کلمه عبور باید حداقل 8 کاراکتر و شامل حروف بزرگ، کوچک، اعداد و کاراکترهای خاص باشد.")]
        public string Password { get; set; }

        [DisplayName("مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
