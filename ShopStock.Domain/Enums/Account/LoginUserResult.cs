namespace ShopStock.Domain.Enums.Account
{
    public enum LoginUserResult
    {
        Success,
        UserNotFound,
        UserNotActive,
        InvalidInputs,
        InvalidPassword,
        EmailNotActivated
    }
}
