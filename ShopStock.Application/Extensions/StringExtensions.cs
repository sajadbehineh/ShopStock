namespace ShopStock.Application.Extensions
{
    public static class StringExtensions
    {
        public static string FixEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            return email.Trim().ToLower();
        }

        public static string FixUserName(this string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return string.Empty;

            return userName.Trim();
        }

        public static string FixMobile(this string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return string.Empty;
            return mobile.Trim();
        }
    }
}
