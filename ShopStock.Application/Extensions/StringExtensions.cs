namespace ShopStock.Application.Extensions
{
    public static class StringExtensions
    {
        extension(string email)
        {
            public string FixEmail()
            {
                if (string.IsNullOrWhiteSpace(email))
                    return string.Empty;

                return email.Trim().ToLower();
            }

            public string FixUserName()
            {
                if (string.IsNullOrWhiteSpace(email))
                    return string.Empty;

                return email.Trim();
            }
        }
    }
}
