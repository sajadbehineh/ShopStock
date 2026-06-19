namespace ShopStock.Domain.Enums.User
{
    public enum EditUserResult
    {
        Success,
        UserNotFound,
        DuplicateUserName,
        DuplicateEmail,
        DuplicateMobile,
        InvalidInputs,
        EditFailed
    }
}
