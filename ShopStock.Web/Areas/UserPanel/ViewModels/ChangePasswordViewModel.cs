using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopStock.Web.Areas.UserPanel.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DisplayName("کلمه عبور فعلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

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
