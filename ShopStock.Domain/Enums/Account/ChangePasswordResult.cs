namespace ShopStock.Domain.Enums.Account
{
    public enum ChangePasswordResult
    {
        Success,
        UserNotFound,
        InvalidCurrentPassword,
        SaveFailed
    }
}
