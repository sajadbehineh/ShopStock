using System.Security.Cryptography;
using System.Text;

namespace ShopStock.Application.Security
{
    public static class PasswordHelper
    {
        public static string HashPassword(this string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return string.Empty;

            // تبدیل متن به بایت با UTF8 (استاندارد و امن تر از Default)
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);

            // استفاده از متد استاتیک، فوق‌العاده سریع و بدون نیاز به Dispose در دات‌نت مدرن
            byte[] hashBytes = SHA256.HashData(inputBytes);

            // تبدیل بایت‌ها به رشته هگزادسیمال بدون خط تیره و با سرعت بالا
            return Convert.ToHexString(hashBytes);
        }

        // Verify if a plain password matches the hashed password
        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            // Hash the plain password
            string hashedInput = plainPassword.HashPassword();

            // مقایسه امن و بدون حساسیت به حروف کوچک و بزرگ
            return string.Equals(hashedInput, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}
