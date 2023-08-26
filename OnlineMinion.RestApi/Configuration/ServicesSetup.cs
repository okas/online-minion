using CorsPolicySettings;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Contracts;
using OnlineMinion.Data.Entities.Shared;
using OnlineMinion.RestApi.MediatorInfra.Behaviors;
using OnlineMinion.RestApi.ProblemHandling;
using OnlineMinion.RestApi.Shared.Handlers;
using OnlineMinion.RestApi.Shared.Requests;

namespace OnlineMinion.RestApi.Configuration;

public static class ServicesSetup
{
    public static IServiceCollection AddRestApi(this IServiceCollection services, IConfigurationRoot config)
    {
        // Order is important (CORS): first read base policies, then produce CORS configuration.
        services.AddCorsPolicies(config);
        services
            .AddSingleton<IConfigureOptions<CorsOptions>, ApiCorsOptionsConfigurator>()
            .AddCors();

        services
            //.ConfigureOptions<MvcOptionsConfigurator>()
            .ConfigureOptions<ApiBehaviorOptionsConfigurator>()
            .AddControllers();

        // Override default one.
        services.AddSingleton<ProblemDetailsFactory, RestApiProblemDetailsFactory>();

        services
            .ConfigureOptions<ApiVersioningOptionsConfigurator>()
            .AddApiVersioning();

        services
            .ConfigureOptions<ApiExplorerOptionsConfigurator>()
            .AddVersionedApiExplorer();

        services.AddEndpointsApiExplorer();

        services.AddValidatorsFromAssemblyContaining<HasIntIdValidator>();
        services.AddValidatorsFromAssembly(typeof(ServicesSetup).Assembly);

        services
            .AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssemblyContaining(typeof(ServicesSetup));

                    // Pipeline
                    cfg.AddOpenBehavior(typeof(CommandValidationBehavior<,>));
                    cfg.AddOpenBehavior(typeof(CommandUnitOfWorkBehavior<,>));
                }
            )
            .AddTransient<
                IRequestHandler<GetPagingMetaInfoReq<Data.Entities.AccountSpec>, PagingMetaInfo>,
                GetPagingInfoReqHlr<Data.Entities.AccountSpec>
            >()
            .AddTransient<
                IRequestHandler<GetPagingMetaInfoReq<BasePaymentSpec>, PagingMetaInfo>,
                GetPagingInfoReqHlr<BasePaymentSpec>
            >();

        return services;
    }
}
