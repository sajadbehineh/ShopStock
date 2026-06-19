namespace ShopStock.Domain.Enums.User
{
    public enum CreateUserResult
    {
        Success,
        SaveFailed,
        EmailDuplicated,
        UserNameDuplicated,
        MobileDuplicated,
        InvalidPicture,
        InvalidData
    }
}