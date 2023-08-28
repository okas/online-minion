using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class UpdateTransactionCreditReqHlr(
        OnlineMinionDbContext                  dbContext,
        ILogger<UpdateTransactionCreditReqHlr> logger
    )
    : BaseUpdateModelReqHlr<UpdateTransactionCreditReq, TransactionCredit>(dbContext, logger)
{
    protected override void UpdateEntityAsync(TransactionCredit entity, UpdateTransactionCreditReq rq)
    {
        entity.Date = rq.Date;
        entity.Amount = rq.Amount;
        entity.Subject = rq.Subject;
        entity.Party = rq.Party;
        entity.PaymentInstrumentId = rq.PaymentInstrumentId;
        entity.Tags = rq.Tags;
    }
}
