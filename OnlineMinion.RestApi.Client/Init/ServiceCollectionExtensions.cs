using Asp.Versioning;
using Asp.Versioning.Http;
using FluentValidation;
using IL.FluentValidation.Extensions.Options;
using Microsoft.Extensions.Options;
using OnlineMinion.Application.Contracts;
using OnlineMinion.RestApi.Client;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Settings;
using OnlineMinion.RestApi.Client.Settings.Validation;
using OnlineMinion.Utilities.Mediatr.Behaviors;

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
        #region AppSettings setup

        services.AddScoped<IValidator<ApiClientSettings>, ApiClientSettingsValidator>();

        services.AddOptions<ApiClientSettings>()
            .BindConfiguration(ApiClientSettings.ConfigurationSection)
            .ValidateWithFluentValidator()
            .ValidateOnStart();

        #endregion

        #region MediatR setup

        services.AddMediatR(
            cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarkerRestApiClient));

                // Pipeline
                cfg.AddOpenBehavior(typeof(ErrorOrLoggingBehavior<,>));
            }
        );

        #endregion

        #region HttpClient setup

        // IApiVersionWriters are singletons, AddApiVersion() is registering QueryStringApiVersionWriter with "Try*".
        // Intention is to only register and use HeaderApiVersionWriter.
        services.AddSingleton<IApiVersionWriter>(new HeaderApiVersionWriter(CustomHeaderNames.ApiVersion));

        var apiHttpClientBuilder = services.AddHttpClient<ApiProvider>(ConfigureApiHttpClient)
            .AddApiVersion(new ApiVersion(1));

        return apiHttpClientBuilder;

        #endregion
    }

    private static void ConfigureApiHttpClient(IServiceProvider sp, HttpClient client) =>
        client.BaseAddress = new(GetApiClientSettings(sp).Url);

    private static ApiClientSettings GetApiClientSettings(IServiceProvider sp) =>
        sp.GetRequiredService<IOptions<ApiClientSettings>>().Value;
}
