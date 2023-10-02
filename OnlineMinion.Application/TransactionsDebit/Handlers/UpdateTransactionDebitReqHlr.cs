using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.TransactionDebits;

namespace OnlineMinion.Application.TransactionsDebit.Handlers;

[UsedImplicitly]
internal sealed class UpdateTransactionDebitReqHlr(
        IOnlineMinionDbContext                dbContext,
        ILogger<UpdateTransactionDebitReqHlr> logger
    )
    : BaseUpdateModelReqHlr<UpdateTransactionDebitReq, TransactionDebit, TransactionDebitId>(dbContext, logger)
{
    protected override TransactionDebitId CreateEntityId(UpdateTransactionDebitReq rq) => new(rq.Id);

    protected override void UpdateEntity(TransactionDebit entity, UpdateTransactionDebitReq rq)
    {
        entity.AccountSpecId = new(rq.AccountSpecId);
        entity.Fee = rq.Fee;
        entity.PaymentInstrumentId = new(rq.PaymentInstrumentId);
        entity.Date = rq.Date;
        entity.Amount = rq.Amount;
        entity.Subject = rq.Subject;
        entity.Party = rq.Party;
        entity.Tags = rq.Tags;
    }
}
