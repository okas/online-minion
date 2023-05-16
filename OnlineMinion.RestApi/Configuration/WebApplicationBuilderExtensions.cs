using CorsPolicySettings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.AppMessaging.Handlers;

namespace OnlineMinion.RestApi.Configuration;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureRestApi(this WebApplicationBuilder webAppBuilder)
    {
        // Order is important (CORS): first read base policies, then produce CORS configuration.
        webAppBuilder.Services.AddCorsPolicies(webAppBuilder.Configuration);
        webAppBuilder.Services
            .AddSingleton<IConfigureOptions<CorsOptions>, ApiCorsOptionsConfigurator>()
            .AddCors();

        webAppBuilder.Services
            // .AddSingleton<IConfigureOptions<MvcOptions>, MvcOptionsConfigurator>()
            .AddControllers();

        webAppBuilder.Services
            .AddSingleton<IConfigureOptions<ApiVersioningOptions>, ApiVersioningOptionsConfigurator>()
            .AddApiVersioning();

        webAppBuilder.Services
            .AddSingleton<IConfigureOptions<ApiExplorerOptions>, ApiExplorerOptionsConfigurator>()
            .AddVersionedApiExplorer();

        webAppBuilder.Services.AddEndpointsApiExplorer();

        webAppBuilder.Services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetPagingInfoReqHlr<>)))
            .AddTransient<
                IRequestHandler<GetPagingMetaInfoReq<AccountSpec>, PagingMetaInfo>,
                GetPagingInfoReqHlr<AccountSpec>
            >();

        return webAppBuilder;
    }
}
