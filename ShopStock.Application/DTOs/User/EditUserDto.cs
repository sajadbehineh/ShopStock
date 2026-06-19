namespace ShopStock.Application.DTOs.User
{
    public class EditUserDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Mobile { get; set; }
        public string? NationalCode { get; set; }
        /// <summary>
        /// در ویرایش، رمز عبور اختیاری است.
        /// اگر مقدار داشته باشد، رمز عبور تغییر می‌کند.
        /// </summary>
        public string? Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<int> UserSelectedRoles { get; set; } = new();

        /// <summary>
        /// انتقال استریم تصویر جدید از کنترلر به سرویس
        /// </summary>
        public Stream? ImageStream { get; set; }

        /// <summary>
        /// نام فایل تصویر فعلی کاربر
        /// </summary>
        public string? CurrentProfilePictureName { get; set; }

        /// <summary>
        /// نام فایل تصویر جدید
        /// </summary>
        public string? ProfilePictureName { get; set; }

        /// <summary>
        /// اگر ادمین بخواهد عکس فعلی کاربر حذف و تصویر پیش‌فرض جایگزین شود.
        /// </summary>
        public bool RemoveCurrentPicture { get; set; }
    }
}
