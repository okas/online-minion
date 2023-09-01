using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class CreateTransactionDebitReqHlr(OnlineMinionDbContext dbContext)
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
