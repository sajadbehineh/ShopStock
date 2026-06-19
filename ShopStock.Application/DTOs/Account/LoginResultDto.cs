using ShopStock.Domain.Enums.Account;

namespace ShopStock.Application.DTOs.Account
{
    public class LoginResultDto
    {
        public LoginUserResult Status { get; set; }
        public UserClaimsDto? User { get; set; }
    }

    public class UserClaimsDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
