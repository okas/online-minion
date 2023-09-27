using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Data;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class CreateTransactionCreditReqHlr(OnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreateTransactionCreditReq, TransactionCredit>(dbContext)
{
    protected override TransactionCredit ToEntity(CreateTransactionCreditReq rq) => new()
    {
        Date = rq.Date,
        Amount = rq.Amount,
        Subject = rq.Subject,
        Party = rq.Party,
        Tags = rq.Tags,
        PaymentInstrumentId = rq.PaymentInstrumentId,
    };
}
