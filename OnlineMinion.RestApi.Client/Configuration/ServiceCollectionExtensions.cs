using System.Runtime.CompilerServices;
using FluentValidation;
using IL.FluentValidation.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using OnlineMinion.RestApi.Client.Infrastructure;
using OnlineMinion.RestApi.Client.Settings;

namespace OnlineMinion.RestApi.Client.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRestApiClient(
        this IServiceCollection services,
        Type[]?                 httpMessageHandlers = null,
        string?                 httpClientName      = null
    )
    {
        CheckHandlerTypes(httpMessageHandlers);

        services.AddValidatorsFromAssemblyContaining(typeof(ServiceCollectionExtensions));

        services.AddOptions<ApiClientProviderSettings>()
            .BindConfiguration(nameof(ApiClientProviderSettings))
            .ValidateWithFluentValidator();

        var httpClientBuilder = string.IsNullOrWhiteSpace(httpClientName)
            ? services.AddHttpClient<ApiClientProvider>()
            : services.AddHttpClient<ApiClientProvider>(httpClientName);

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
            opts => opts.RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions))
        );

        return services;
    }

    private static void CheckHandlerTypes(
        Type[]? httpMessageHandlers,
        [CallerArgumentExpression(nameof(httpMessageHandlers))]
        string? paramName = null
    )
    {
        if (httpMessageHandlers is not null &&
            httpMessageHandlers.Any(type => !type.IsSubclassOf(typeof(DelegatingHandler))))
        {
            throw new ArgumentException(
                "All handlers must derive from DelegatingHandler.",
                nameof(httpMessageHandlers)
            );
        }
    }
}
