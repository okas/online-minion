using CorsPolicySettings;
using Microsoft.Extensions.Configuration;
using OnlineMinion.RestApi.Configuration;
using OnlineMinion.RestApi.ProblemHandling;
using OnlineMinion.RestApi.Services.LinkGeneration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRestApi(this IServiceCollection services, IConfigurationRoot config)
    {
        #region CORS

        // Order is important (CORS): first read base policies, then produce CORS configuration.
        services.AddCorsPolicies(config);
        services.ConfigureOptions<ApiCorsOptionsConfigurator>().AddCors();

        #endregion

        #region API general setup

        services.ConfigureOptions<RouteOptionsConfigurator>();

        services.AddHttpContextAccessor().AddScoped<ResourceLinkGenerator>();

        #endregion

        #region API Problem handling setup

        services.AddSingleton<IExceptionProblemDetailsMapper, ApiExceptionProblemDetailsMapper>()
            .ConfigureOptions<ProblemDetailsOptionsConfigurator>()
            .AddProblemDetails();

        #endregion

        # region API versioning setup

        services.ConfigureOptions<ApiVersioningOptionsConfigurator>()
            .ConfigureOptions<ApiExplorerOptionsConfigurator>()
            .AddEndpointsApiExplorer()
            .AddApiVersioning()
            .AddApiExplorer();

        #endregion

        #region Application services

        services.AddApplication();

        #endregion

        return services;
    }
}
