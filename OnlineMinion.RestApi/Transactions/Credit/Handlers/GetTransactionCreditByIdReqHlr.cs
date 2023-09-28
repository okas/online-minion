using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Contracts.Transactions.Credit.Responses;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Credit.Handlers;

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
