using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineMinion.RestApi.Client.Infrastructure;
using OnlineMinion.RestApi.Client.Settings;

namespace OnlineMinion.RestApi.Client.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRestApiClient(
        this IServiceCollection         services,
        IConfigurationRoot              config,
        IEnumerable<DelegatingHandler>? httpMessageHandlers = null,
        string?                         httpClientName      = null
    )
    {
        services.Configure<ApiClientProviderSettings>(config.GetSection(nameof(ApiClientProviderSettings)));

        var httpClientBuilder = string.IsNullOrWhiteSpace(httpClientName)
            ? services.AddHttpClient<ApiClientProvider>()
            : services.AddHttpClient<ApiClientProvider>(httpClientName);

        if (httpMessageHandlers is not null)
        {
            foreach (var delegatingHandler in httpMessageHandlers)
            {
                services.AddTransient(delegatingHandler.GetType());
                httpClientBuilder.AddHttpMessageHandler(() => delegatingHandler);
            }
        }

        services.AddMediatR(
            opts => opts.RegisterServicesFromAssemblyContaining<ApiClientProvider>()
        );

        return services;
    }
}
