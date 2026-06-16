using Microsoft.Extensions.DependencyInjection;
using ShopStock.Domain.Interfaces;
using ShopStock.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using ShopStock.Infra.Data.Context;

namespace ShopStock.Infra.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<EshopDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            return services;
        }
    }
}