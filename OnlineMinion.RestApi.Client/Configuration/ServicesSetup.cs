using System.Runtime.CompilerServices;
using FluentValidation;
using IL.FluentValidation.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Settings;

namespace OnlineMinion.RestApi.Client.Configuration;

public static class ServicesSetup
{
    public static IServiceCollection AddRestApiClient(
        this IServiceCollection services,
        Type[]?                 httpMessageHandlers = null,
        string?                 httpClientName      = null
    )
    {
        CheckHandlerTypes(httpMessageHandlers);

        services.AddValidatorsFromAssemblyContaining(typeof(ServicesSetup));

        services.AddOptions<ApiClientProviderSettings>()
            .BindConfiguration(nameof(ApiClientProviderSettings))
            .ValidateWithFluentValidator();

        var httpClientBuilder = services.AddHttpClient<ApiClientProvider>(
            string.IsNullOrWhiteSpace(httpClientName)
                ? nameof(ApiClientProvider)
                : httpClientName
        );

        if (httpMessageHandlers is not null)
        {
            foreach (var delegatingHandler in httpMessageHandlers.Distinct())
            {
                services.AddTransient(delegatingHandler);
                httpClientBuilder.AddHttpMessageHandler(
                    sp => (sp.GetRequiredService(delegatingHandler) as DelegatingHandler)!
                );
            }
        }

        services.AddMediatR(
            opts => opts.RegisterServicesFromAssemblyContaining(typeof(ServicesSetup))
        );

        return services;
    }

    private static void CheckHandlerTypes(
        Type[]? httpMessageHandlers,
        [CallerArgumentExpression(nameof(httpMessageHandlers))]
        string? paramName = null
    )
    {
        if (httpMessageHandlers is not null
            && httpMessageHandlers.Any(type => !type.IsSubclassOf(typeof(DelegatingHandler))))
        {
            throw new ArgumentException(
                "All handlers must derive from DelegatingHandler.",
                paramName
            );
        }
    }
}
