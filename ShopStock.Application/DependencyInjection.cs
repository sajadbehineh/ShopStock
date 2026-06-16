using Microsoft.Extensions.DependencyInjection;
using ShopStock.Application.Services.Interfaces;
using ShopStock.Application.Services.Implementations;

namespace ShopStock.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //// ثبت سرویس‌های لایه اپلیکیشن
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IImageService, ImageService>();

            return services;
        }
    }
}
