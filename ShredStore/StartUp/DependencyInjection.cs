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
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddTransient<IUserHttpService, UserHttpService>();
            services.AddTransient<IProductHttpService, ProductHttpService>();
            services.AddTransient<ICartHttpService, CartHttpService>();
            services.AddTransient<ICartItemHttpService, CartItemHttpService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();


            return services;

        }
    }
}
