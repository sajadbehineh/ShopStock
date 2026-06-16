using System.ComponentModel;

namespace ShopStock.Web.Areas.Admin.ViewModels.User
{
    public class DeleteUserViewModel
    {
        public int Id { get; set; }

        [DisplayName("نام")]
        public string? FirstName { get; set; }

        [DisplayName("نام خانوادگی")]
        public string? LastName { get; set; }

        [DisplayName("نام کاربری")]
        public string UserName { get; set; }

        [DisplayName("تصویر پروفایل")]
        public string? ProfilePicture { get; set; }

        [DisplayName("نقش‌ها")]
        public List<string> Roles { get; set; } = new();
    }
}