namespace ShopStock.Domain.Enums.User
{
    public enum CreateUserResult
    {
        Success,
        SaveFailed,
        EmailRequired,
        UserNameRequired,
        PasswordRequired,
        EmailDuplicated,
        UserNameDuplicated,
        MobileDuplicated,
        InvalidPicture,
        RoleRequired,
        InvalidImage
    }
}