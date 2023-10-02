using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Responses;
using OnlineMinion.Application.Shared.Handlers;

namespace OnlineMinion.Application.TransactionCredit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionCreditsReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetSomeModelsPagedReqHlr<Domain.TransactionCredits.TransactionCredit, TransactionCreditResp>(dbContext)
{
    protected override Expression<Func<Domain.TransactionCredits.TransactionCredit, TransactionCreditResp>>
        Projection =>
        entity => new(
            entity.Id.Value,
            entity.PaymentInstrumentId.Value,
            entity.Date,
            entity.Amount,
            entity.Subject,
            entity.Party,
            entity.Tags
        );
}
