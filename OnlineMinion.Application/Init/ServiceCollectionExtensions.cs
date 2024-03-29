using ErrorOr;
using MediatR;
using OnlineMinion.Application;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Application.MediatorInfra.Behaviors;
using OnlineMinion.Application.RequestValidation;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain;
using OnlineMinion.Domain.Shared;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        #region API FluentValidation setup

        services.AddApplicationRequestValidation();

        #endregion

        # region API MediatR setup

        services
            .AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarkerApplication));

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

        services.AddTransient<IAsyncValidatorSender, MediatorDecorator>();

        #endregion

        return services;
    }
}
