using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.Shared.Handlers;

namespace OnlineMinion.Application.TransactionCredit.Handlers;

[UsedImplicitly]
internal sealed class CreateTransactionCreditReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreateTransactionCreditReq, Domain.TransactionCredits.TransactionCredit>(dbContext)
{
    protected override Domain.TransactionCredits.TransactionCredit ToEntity(CreateTransactionCreditReq rq) => new()
    {
        Date = rq.Date,
        Amount = rq.Amount,
        Subject = rq.Subject,
        Party = rq.Party,
        Tags = rq.Tags,
        PaymentInstrumentId = new(rq.PaymentInstrumentId),
    };
}
