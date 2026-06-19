namespace ShopStock.Domain.Enums.Account
{
    public enum RegisterUserResult
    {
        Success,
        InvalidInputs,
        EmailDuplicated,
        UserNameDuplicated,
        ActivationEmailSent,
        Failed,
        FailedActivationEmailSending
    }
}
