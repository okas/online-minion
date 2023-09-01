using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionCreditsReqHlr(OnlineMinionDbContext dbContext)
    : BaseGetSomeModelsPagedReqHlr<TransactionCredit, TransactionCreditResp>(dbContext)
{
    protected override Expression<Func<TransactionCredit, TransactionCreditResp>> Projection =>
        e => new(
            e.Id,
            e.Date,
            e.Amount,
            e.Subject,
            e.Party,
            e.PaymentInstrumentId,
            e.Tags
        );
}
