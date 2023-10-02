using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.TransactionDebits;

namespace OnlineMinion.Application.TransactionsDebit.Handlers;

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
        PaymentInstrumentId = new(rq.PaymentInstrumentId),
        AccountSpecId = new(rq.AccountSpecId),
    };
}
