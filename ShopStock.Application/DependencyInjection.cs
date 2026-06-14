using Microsoft.Extensions.DependencyInjection;
using ShopStock.Application.Contracts;
using ShopStock.Application.Services;

namespace ShopStock.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //// ثبت سرویس‌های لایه اپلیکیشن
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IImageService, ImageService>();

            return services;
        }
    }
}
