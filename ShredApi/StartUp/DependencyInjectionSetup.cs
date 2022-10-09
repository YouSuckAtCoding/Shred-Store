using DataLibrary.DataAccess;
using DataLibrary.Product;
using DataLibrary.User;

namespace ShredApi.StartUp
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();

            return services;
        }
    }
}
