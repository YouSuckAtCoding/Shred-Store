using ShredStore.Models.Utility;
using ShredStore.Services;

namespace ShredStore.StartUp
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection RegisterServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;
            
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
            services.AddTransient<ListCorrector>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "ShredStore_";
            });




            return services;

        }
    }
}
