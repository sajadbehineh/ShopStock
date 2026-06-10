namespace ShopStock.Application.Generators
{
    public static class TokenGenerator
    {
        public static string GenerateUniqueToken() => Guid.NewGuid().ToString("N");
    }
}
