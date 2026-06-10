using System;
using System.Collections.Generic;
using System.Text;

namespace ShopStock.Application.DTOs.Account
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
