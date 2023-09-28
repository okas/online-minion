using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionDebitsReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetSomeModelsPagedReqHlr<TransactionDebit, TransactionDebitResp>(dbContext)
{
    protected override Expression<Func<TransactionDebit, TransactionDebitResp>> Projection =>
        entity => new(
            entity.Id,
            entity.PaymentInstrumentId,
            entity.AccountSpecId,
            entity.Fee,
            entity.Date,
            entity.Amount,
            entity.Subject,
            entity.Party,
            entity.Tags
        );
}
