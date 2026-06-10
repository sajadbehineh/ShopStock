using ShopStock.Domain.Entities.Common;

namespace ShopStock.Domain.Entities.Users;

public class User : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string EmailActiveCode { get; set; }
    public bool IsEmailActive { get; set; }
    public string? Mobile { get; set; }
    public int? MobileActiveCode { get; set; }
    public string? NationalCode { get; set; }
    public string PasswordHash { get; set; }
    public string? ProfilePicture { get; set; }
    public bool IsActive { get; set; }

    #region Relations
    public ICollection<UserAddress> UserAddresses { get; set; }
    #endregion
}