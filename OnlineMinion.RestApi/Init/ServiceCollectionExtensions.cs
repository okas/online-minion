using CorsPolicySettings;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using OnlineMinion.Application;
using OnlineMinion.Common.Validation;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Domain;
using OnlineMinion.Domain.Shared;
using OnlineMinion.RestApi;
using OnlineMinion.RestApi.Configuration;
using OnlineMinion.RestApi.MediatorInfra.Behaviors;
using OnlineMinion.RestApi.ProblemHandling;
using OnlineMinion.RestApi.Services.LinkGeneration;
using OnlineMinion.RestApi.Shared.Handlers;

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

        #region API FluentValidatior setup

        services.AddValidatorsFromAssemblyContaining<IAssemblyMarkerCommonValidation>();
        services.AddValidatorsFromAssembly(typeof(IAssemblyMarkerRestApi).Assembly);

        #endregion

        # region API MediatR setup

        services
            .AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarkerRestApi));

                    // Pipeline
                    cfg.AddOpenBehavior(typeof(CommandValidationBehavior<,>));
                    cfg.AddOpenBehavior(typeof(CommandUnitOfWorkBehavior<,>));
                }
            )
            // TODO: Better implement class based handlers to avoid these registrations.
            .AddTransient<IRequestHandler<GetAccountSpecPagingMetaInfoReq, ErrorOr<PagingMetaInfo>>,
                GetModelPagingInfoReqHlr<GetAccountSpecPagingMetaInfoReq, AccountSpec>
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

        #region Application services

        services.AddApplication();

        #endregion

        return services;
    }
}
