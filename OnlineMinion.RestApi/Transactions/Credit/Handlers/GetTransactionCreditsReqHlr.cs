using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.Transactions.Credit.Responses;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionCreditsReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetSomeModelsPagedReqHlr<TransactionCredit, TransactionCreditResp>(dbContext)
{
    protected override Expression<Func<TransactionCredit, TransactionCreditResp>> Projection =>
        entity => new(
            entity.Id,
            entity.PaymentInstrumentId,
            entity.Date,
            entity.Amount,
            entity.Subject,
            entity.Party,
            entity.Tags
        );
}
