using CorsPolicySettings;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Data.Entities;
using OnlineMinion.Data.Entities.Shared;
using OnlineMinion.RestApi.MediatorInfra.Behaviors;
using OnlineMinion.RestApi.ProblemHandling;
using OnlineMinion.RestApi.Shared.Handlers;

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
            .AddTransient<IRequestHandler<GetAccountPagingMetaInfoReq, ErrorOr<PagingMetaInfo>>,
                GetModelPagingInfoReqHlr<GetAccountPagingMetaInfoReq, Data.Entities.AccountSpec>
            >()
            .AddTransient<IRequestHandler<GetPaymentSpecPagingMetaInfoReq, ErrorOr<PagingMetaInfo>>,
                GetModelPagingInfoReqHlr<GetPaymentSpecPagingMetaInfoReq, BasePaymentSpec>
            >()
            .AddTransient<IRequestHandler<GetTransactionCreditPagingMetaInfoReq, ErrorOr<PagingMetaInfo>>,
                GetModelPagingInfoReqHlr<GetTransactionCreditPagingMetaInfoReq, TransactionCredit>
            >();

        return services;
    }
}
