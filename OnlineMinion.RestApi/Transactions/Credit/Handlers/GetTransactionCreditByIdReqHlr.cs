using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionCreditByIdReqHlr(OnlineMinionDbContext dbContext)
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
