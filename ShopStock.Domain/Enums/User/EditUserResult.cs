namespace ShopStock.Domain.Enums.User
{
    public enum EditUserResult
    {
        Success,
        UserNotFound,
        EmailRequired,
        UserNameRequired,
        UserNameDuplicated,
        EmailDuplicated,
        MobileDuplicated,
        EditFailed,
        RoleRequired,
        InvalidImage
    }
}
