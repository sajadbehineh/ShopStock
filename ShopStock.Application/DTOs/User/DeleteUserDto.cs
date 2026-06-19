namespace ShopStock.Application.DTOs.User
{
    public class DeleteUserDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }
        public string? ProfilePicture { get; set; }
        public List<string> UserSelectedRolesName { get; set; } = new();
    }
}
