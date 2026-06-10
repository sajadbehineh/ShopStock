using System;
using System.Collections.Generic;
using System.Text;

namespace ShopStock.Domain.Enums
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
