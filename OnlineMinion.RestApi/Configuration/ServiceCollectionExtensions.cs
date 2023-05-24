using CorsPolicySettings;
using MediatR;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OnlineMinion.Contracts;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.AppMessaging.Handlers;
using OnlineMinion.RestApi.AppMessaging.Requests;
using OnlineMinion.RestApi.ProblemHandling;

namespace OnlineMinion.RestApi.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRestApi(this IServiceCollection services, IConfigurationRoot config)
    {
        // Order is important (CORS): first read base policies, then produce CORS configuration.
        services.AddCorsPolicies(config);
        services
            .AddSingleton<IConfigureOptions<CorsOptions>, ApiCorsOptionsConfigurator>()
            .AddCors();

        services
            // .AddSingleton<IConfigureOptions<MvcOptions>, MvcOptionsConfigurator>()
            .AddControllers();

        // Override default one.
        services.AddSingleton<ProblemDetailsFactory, RestApiProblemDetailsFactory>();

        services
            .AddSingleton<IConfigureOptions<ApiVersioningOptions>, ApiVersioningOptionsConfigurator>()
            .AddApiVersioning();

        services
            .AddSingleton<IConfigureOptions<ApiExplorerOptions>, ApiExplorerOptionsConfigurator>()
            .AddVersionedApiExplorer();

        services.AddEndpointsApiExplorer();

        services
            .AddMediatR(
                cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetPagingInfoReqHlr<>))
            )
            .AddTransient<
                IRequestHandler<GetPagingMetaInfoReq<AccountSpec>, PagingMetaInfo>,
                GetPagingInfoReqHlr<AccountSpec>
            >();

        return services;
    }
}
