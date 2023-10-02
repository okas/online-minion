using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.Contracts.Transactions.Credit.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.TransactionCredits;

namespace OnlineMinion.Application.TransactionCredit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionCreditByIdReqHlr(IOnlineMinionDbContext dbContext) : BaseGetModelByIdReqHlr<
    GetTransactionCreditByIdReq,
    Domain.TransactionCredits.TransactionCredit,
    TransactionCreditId,
    TransactionCreditResp
>(dbContext)
{
    protected override TransactionCreditId CreateEntityId(GetTransactionCreditByIdReq rq) => new(rq.Id);

    protected override TransactionCreditResp ToResponse(Domain.TransactionCredits.TransactionCredit entity) => new(
        entity.Id.Value,
        entity.PaymentInstrumentId.Value,
        entity.Date,
        entity.Amount,
        entity.Subject,
        entity.Party,
        entity.Tags
    );
}
