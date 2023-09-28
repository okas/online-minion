using Asp.Versioning;
using Asp.Versioning.Http;
using FluentValidation;
using IL.FluentValidation.Extensions.Options;
using Microsoft.Extensions.Options;
using OnlineMinion.Contracts;
using OnlineMinion.RestApi.Client;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Settings;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>Sets up API Client, configurations and related services.</summary>
    /// <param name="services"></param>
    /// <returns>
    ///     The instance of the <see cref="IHttpClientBuilder" /> to allow further configuration of client builder.
    ///     This instance is used to build the API Client.
    /// </returns>
    public static IHttpClientBuilder AddRestApiClient(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(IAssemblyMarkerRestApiClient));

        services.AddOptions<ApiClientSettings>()
            .BindConfiguration(nameof(ApiClientSettings))
            .ValidateWithFluentValidator();

        // IApiVersionWriters are singletons, AddApiVersion() is registering QueryStringApiVersionWriter with "Try*".
        // Intention is to only register and use HeaderApiVersionWriter.
        var apiHttpClientBuilder = services
            .AddSingleton<IApiVersionWriter>(new HeaderApiVersionWriter(CustomHeaderNames.ApiVersion))
            .AddHttpClient<ApiProvider>(ConfigureApiHttpClient)
            .AddApiVersion(new ApiVersion(1));

        services.AddMediatR(
            opts => opts.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarkerRestApiClient))
        );

        return apiHttpClientBuilder;
    }

    private static void ConfigureApiHttpClient(IServiceProvider sp, HttpClient client) =>
        client.BaseAddress = new(GetApiClientSettings(sp).Url);

    private static ApiClientSettings GetApiClientSettings(IServiceProvider sp) =>
        sp.GetRequiredService<IOptions<ApiClientSettings>>().Value;
}
