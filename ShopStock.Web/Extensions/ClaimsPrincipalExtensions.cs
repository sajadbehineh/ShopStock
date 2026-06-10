using System.Security.Claims;

namespace ShopStock.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID claim not found.");

            return int.Parse(userIdClaim.Value);
        }

        // اگر در آینده نیاز به استفاده از Guid به جای int برای شناسه کاربر داشتید، می‌توانید این متد را فعال کنید:

        //public static Guid GetUserId(this ClaimsPrincipal user)
        //{
        //    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        //    if (userIdClaim == null)
        //        throw new UnauthorizedAccessException("User ID claim not found.");

        //    return Guid.Parse(userIdClaim.Value);
        //}

        // یا اگر می‌خواهید یک نسخه امن‌تر داشته باشید که در صورت عدم وجود کلایم یا عدم توانایی تبدیل، مقدار null برگرداند:

        //public static int? GetUserId(this ClaimsPrincipal user)
        //{
        //    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        //    if (userIdClaim == null)
        //        return null;

        //    return int.TryParse(userIdClaim.Value, out var id) ? id : null;
        //}
    }
}
