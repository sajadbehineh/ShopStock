namespace ShopStock.Domain.Enums.User
{
    public enum AdminCreateUserResult
    {
        Success,
        Error,
        EmailDuplicated,
        UserNameDuplicated,
        MobileDuplicated,
        InvalidPicture,
        InvalidData
    }
}