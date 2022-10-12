using ShredStore.Services;

namespace ShredStore.StartUp
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddControllersWithViews();
            services.AddTransient<IUserHttpService, UserHttpService>();
            services.AddTransient<IProductHttpService, ProductHttpService>();
            services.AddTransient<ICartHttpService, CartHttpService>();
            services.AddTransient<ICartItemHttpService, CartItemHttpService>();


            return services;

        }
    }
}
