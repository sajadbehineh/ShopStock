using ShopStock.Domain.Entities.Roles;

namespace ShopStock.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
    }
}
