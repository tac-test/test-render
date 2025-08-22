using Microsoft.EntityFrameworkCore;
using TestRender.Data;

namespace TestRender
{
    public static class SerivceConfigurations
    {
        public static IServiceCollection AddServiceConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabaseConfigurations(configuration);
            services.AddCachingConfigurations(configuration);
            return services;
        }
        private static IServiceCollection AddDatabaseConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("Postgres"));
            });
            return services;
        }
        private static IServiceCollection AddCachingConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "TestRender:"; // Optional: for key prefixing
            });
            return services;
        }
    }
}
