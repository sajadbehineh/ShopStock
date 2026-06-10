namespace ShopStock.Application.DTOs.Account
{
    public class EditProfileDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string? NationalCode { get; set; }

        // انتقال استریم تصویر از Controller به Service
        public Stream? ImageStream { get; set; }
        public string? CurrentProfilePicture { get; set; }
        public bool RemoveCurrentPicture { get; set; }
    }
}
