using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class UpdateTransactionCreditReqHlr(
        IOnlineMinionDbContext                 dbContext,
        ILogger<UpdateTransactionCreditReqHlr> logger
    )
    : BaseUpdateModelReqHlr<UpdateTransactionCreditReq, TransactionCredit>(dbContext, logger)
{
    protected override void UpdateEntityAsync(TransactionCredit entity, UpdateTransactionCreditReq rq)
    {
        entity.PaymentInstrumentId = rq.PaymentInstrumentId;
        entity.Date = rq.Date;
        entity.Amount = rq.Amount;
        entity.Subject = rq.Subject;
        entity.Party = rq.Party;
        entity.Tags = rq.Tags;
    }
}
