using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.TransactionCredits;

namespace OnlineMinion.Application.TransactionCredit.Handlers;

[UsedImplicitly]
internal sealed class UpdateTransactionCreditReqHlr(
        IOnlineMinionDbContext                 dbContext,
        ILogger<UpdateTransactionCreditReqHlr> logger
    )
    : BaseUpdateModelReqHlr<
        UpdateTransactionCreditReq,
        Domain.TransactionCredits.TransactionCredit,
        TransactionCreditId
    >(dbContext, logger)
{
    protected override TransactionCreditId CreateEntityId(UpdateTransactionCreditReq rq) => new(rq.Id);

    protected override void UpdateEntity(
        Domain.TransactionCredits.TransactionCredit entity,
        UpdateTransactionCreditReq                  rq
    )
    {
        entity.PaymentInstrumentId = new(rq.PaymentInstrumentId);
        entity.Date = rq.Date;
        entity.Amount = rq.Amount;
        entity.Subject = rq.Subject;
        entity.Party = rq.Party;
        entity.Tags = rq.Tags;
    }
}
