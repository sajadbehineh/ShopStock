using System;
using System.Collections.Generic;
using System.Text;

namespace ShopStock.Domain.Enums
{
    public enum RegisterUserResult
    {
        Success,
        InvalidInputs,
        EmailAlreadyExists,
        UserNameAlreadyExists,
        ActivationEmailSent,
        Failed,
        FailedActivationEmailSending
    }
}
