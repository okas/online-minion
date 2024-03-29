using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Transactions.Debit.Handlers;

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
