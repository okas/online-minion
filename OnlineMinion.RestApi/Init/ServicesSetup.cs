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
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Data.Entities;
using OnlineMinion.Data.Entities.Shared;
using OnlineMinion.RestApi.MediatorInfra.Behaviors;
using OnlineMinion.RestApi.ProblemHandling;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Init;

public static class ServicesSetup
{
    public static IServiceCollection AddRestApi(this IServiceCollection services, IConfigurationRoot config)
    {
        #region CORS

        // Order is important (CORS): first read base policies, then produce CORS configuration.
        services.AddCorsPolicies(config);
        services
            .AddSingleton<IConfigureOptions<CorsOptions>, ApiCorsOptionsConfigurator>()
            .AddCors();

        #endregion

        #region API general setup

        services.ConfigureOptions<ApiBehaviorOptionsConfigurator>();
        services.ConfigureOptions<RouteOptionsConfigurator>();

        services.AddControllers();

        #endregion

        #region API Problem handling setup

        // Override default one.
        services.AddSingleton<ProblemDetailsFactory, RestApiProblemDetailsFactory>();
        services.AddProblemDetails(); // For MinimalAPI

        #endregion

        # region API versioning setup

        services
            .ConfigureOptions<ApiVersioningOptionsConfigurator>()
            .ConfigureOptions<ApiExplorerOptionsConfigurator>()
            .AddEndpointsApiExplorer()
            .AddApiVersioning()
            .AddApiExplorer();

        #endregion

        #region API FluentValidatior setup

        services.AddValidatorsFromAssemblyContaining<HasIntIdValidator>();
        services.AddValidatorsFromAssembly(typeof(ServicesSetup).Assembly);

        #endregion

        # region API MediatR setup

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
            // TODO: Better implement class based handlers to avoid these registrations.
            .AddTransient<IRequestHandler<GetAccountSpecPagingMetaInfoReq, ErrorOr<PagingMetaInfo>>,
                GetModelPagingInfoReqHlr<GetAccountSpecPagingMetaInfoReq, Data.Entities.AccountSpec>
            >()
            .AddTransient<IRequestHandler<GetPaymentSpecPagingMetaInfoReq, ErrorOr<PagingMetaInfo>>,
                GetModelPagingInfoReqHlr<GetPaymentSpecPagingMetaInfoReq, BasePaymentSpec>
            >()
            .AddTransient<IRequestHandler<GetTransactionCreditPagingMetaInfoReq, ErrorOr<PagingMetaInfo>>,
                GetModelPagingInfoReqHlr<GetTransactionCreditPagingMetaInfoReq, TransactionCredit>
            >()
            .AddTransient<IRequestHandler<GetTransactionDebitPagingMetaInfoReq, ErrorOr<PagingMetaInfo>>,
                GetModelPagingInfoReqHlr<GetTransactionDebitPagingMetaInfoReq, TransactionDebit>
            >();

        #endregion

        return services;
    }
}
