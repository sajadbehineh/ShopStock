namespace ShopStock.Domain.Enums.User
{
    public enum CreateUserResult
    {
        Success,
        SaveFailed,
        RoleSaveFailed,
        EmailDuplicated,
        UserNameDuplicated,
        MobileDuplicated,
        InvalidPicture,
        InvalidData
    }
}