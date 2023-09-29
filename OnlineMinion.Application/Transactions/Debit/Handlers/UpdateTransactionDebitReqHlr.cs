using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class UpdateTransactionDebitReqHlr(
        IOnlineMinionDbContext                dbContext,
        ILogger<UpdateTransactionDebitReqHlr> logger
    )
    : BaseUpdateModelReqHlr<UpdateTransactionDebitReq, TransactionDebit>(dbContext, logger)
{
    protected override void UpdateEntityAsync(TransactionDebit entity, UpdateTransactionDebitReq rq)
    {
        entity.AccountSpecId = rq.AccountSpecId;
        entity.Fee = rq.Fee;
        entity.PaymentInstrumentId = rq.PaymentInstrumentId;
        entity.Date = rq.Date;
        entity.Amount = rq.Amount;
        entity.Subject = rq.Subject;
        entity.Party = rq.Party;
        entity.Tags = rq.Tags;
    }
}
