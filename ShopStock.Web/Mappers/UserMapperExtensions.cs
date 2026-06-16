using ShopStock.Application.DTOs.User;
using ShopStock.Web.Areas.Admin.ViewModels.User;

namespace ShopStock.Web.Mappers
{
    public static class UserMapperExtensions
    {
        public static UserListViewModel? MapToViewModel(this UserListDto dto)
        {
            if (dto == null) return null;

            return new UserListViewModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email,
                Mobile = dto.Mobile,
                NationalCode = dto.NationalCode,
                ProfilePicture = dto.ProfilePicture,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

        public static IEnumerable<UserListViewModel?> MapToListViewModels(this IEnumerable<UserListDto> dtos)
        {
            if (dtos == null) return new List<UserListViewModel>();

            return dtos.Select(dto => dto.MapToViewModel()).ToList();
        }

        public static DeleteUserViewModel? MapToDeleteViewModel(this DeleteUserDto dto)
        {
            if (dto == null) return null;

            return new DeleteUserViewModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                ProfilePicture = dto.ProfilePicture,
                Roles = dto.Roles
            };
        }
    }
}
