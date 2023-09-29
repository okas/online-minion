using JetBrains.Annotations;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class CreateTransactionDebitReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreateTransactionDebitReq, TransactionDebit>(dbContext)
{
    protected override TransactionDebit ToEntity(CreateTransactionDebitReq rq) => new()
    {
        Fee = rq.Fee,
        Date = rq.Date,
        Amount = rq.Amount,
        Subject = rq.Subject,
        Party = rq.Party,
        Tags = rq.Tags,
        PaymentInstrumentId = rq.PaymentInstrumentId,
        AccountSpecId = rq.AccountSpecId,
    };
}
