using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.Contracts.Transactions.Credit.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionCreditByIdReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetModelByIdReqHlr<GetTransactionCreditByIdReq, TransactionCredit, TransactionCreditResp>(dbContext)
{
    protected override TransactionCreditResp ToResponse(TransactionCredit entity) => new(
        entity.Id,
        entity.PaymentInstrumentId,
        entity.Date,
        entity.Amount,
        entity.Subject,
        entity.Party,
        entity.Tags
    );
}
