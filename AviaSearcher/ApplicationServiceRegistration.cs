


using AviaSearcher.AviaSearchService;
using WebAPI.Clients;

namespace AviaSearcher
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IAviaSearchService, AviaSearchService.AviaSearchService>();

            services.AddHttpClient<IAviaDataClient, AviaDataClient>();
            services.AddScoped<IAviaDataClient, AviaDataClient>();
            services.AddHttpClient<IAviaData2Client, AviaData2Client>();
            services.AddScoped<IAviaData2Client, AviaData2Client>();


            return services;
        }
    }
}
