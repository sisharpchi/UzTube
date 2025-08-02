using Application.Dtos.MegaSettings;
using CG.Web.MegaApiClient;

namespace Api.Configurations;

public static class MegaServiceRegistration
{
    public static IServiceCollection AddMegaIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. MegaSettings ni o‘qish
        services.Configure<MegaSettings>(
            configuration.GetSection("Mega"));

        // 2. MegaApiClient ni singleton sifatida register qilish
        services.AddSingleton(provider =>
        {
            var settings = configuration.GetSection("Mega").Get<MegaSettings>();
            var client = new MegaApiClient();
            client.Login(settings!.Email, settings.Password);
            return client;
        });


        return services;
    }
}
