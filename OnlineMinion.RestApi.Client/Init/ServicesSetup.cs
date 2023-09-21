using FluentValidation;
using IL.FluentValidation.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.HttpRequestMessageHandlers;
using OnlineMinion.RestApi.Client.Settings;

namespace OnlineMinion.RestApi.Client.Init;

public static class ServicesSetup
{
    /// <summary>Sets up API Client, configurations and related services.</summary>
    /// <param name="services"></param>
    /// <returns>
    ///     The instance of the <see cref="IHttpClientBuilder" /> to allow further configuration of client builder.
    ///     This instance is used to build the API Client.
    /// </returns>
    public static IHttpClientBuilder AddRestApiClient(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(ServicesSetup));

        services.AddOptions<ApiProviderSettings>()
            .BindConfiguration(nameof(ApiProviderSettings))
            .ValidateWithFluentValidator();

        var apiHttpClientBuilder = services.AddTransient<DefaultApiVersionHeaderRequestHandler>()
            .AddHttpClient<ApiProvider>(ConfigureApiHttpClient)
            .AddHttpMessageHandler<DefaultApiVersionHeaderRequestHandler>();

        services.AddMediatR(
            opts => opts.RegisterServicesFromAssemblyContaining(typeof(ServicesSetup))
        );

        return apiHttpClientBuilder;
    }

    private static void ConfigureApiHttpClient(IServiceProvider serviceProvider, HttpClient client)
    {
        var settings = serviceProvider.GetRequiredService<IOptions<ApiProviderSettings>>().Value;
        client.BaseAddress = new(settings.Url);
    }
}
