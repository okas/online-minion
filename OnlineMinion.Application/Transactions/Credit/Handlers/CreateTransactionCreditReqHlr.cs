using JetBrains.Annotations;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class CreateTransactionCreditReqHlr(IOnlineMinionDbContext dbContext)
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
