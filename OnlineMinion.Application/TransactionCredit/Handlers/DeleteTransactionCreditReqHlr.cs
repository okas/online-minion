using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.TransactionCredits;

namespace OnlineMinion.Application.TransactionCredit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionCreditReqHlr(IOnlineMinionDbContext dbContext) : BaseDeleteModelReqHlr<
    DeleteTransactionCreditReq,
    Domain.TransactionCredits.TransactionCredit,
    TransactionCreditId
>(dbContext)
{
    protected override TransactionCreditId CreateEntityId(DeleteTransactionCreditReq rq) => new(rq.Id);
}
