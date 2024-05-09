using Microsoft.Extensions.DependencyInjection.Extensions;
using RecommendationManagementService.Business.Implementation;
using RecommendationManagementService.Business.Interface;
using RecommendationManagementService.Core.AppSettings;
using RecommendationManagementService.Data.Implementation;
using RecommendationManagementService.Data.Interface;

namespace RecommendationManagementService.API
{
    public static class ServicesConfigurator
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureAppSettings(configuration);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserResolver, UserResolver>();

            services.ConfigureDataAccessServices();

            services.ConfigureBusinessServices();
        }

        private static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Auth0Settings>(configuration.GetSection("Auth0"));
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
        }

        private static void ConfigureDataAccessServices(this IServiceCollection services)
        {
            //services.AddScoped<IBaseServiceDataAccess, BaseServiceDataAccess>();
            services.AddScoped<IRecommendationServiceDataAccess, RecommendationServiceDataAccess>();
        }

        private static void ConfigureBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IRecommendationService, RecommendationService>();
        }
    }
}
