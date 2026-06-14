using ShopStock.Domain.Entities.Roles;

namespace ShopStock.Application.DTOs.User
{
    public class AdminCreateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Mobile { get; set; }
        public string? NationalCode { get; set; }
        public string Password { get; set; }
        public string? ProfilePictureName { get; set; }
        public Stream? ImageStream { get; set; }
        public bool IsActive { get; set; }

        public List<int>? UserSelectedRoles { get; set; }
    }
}
