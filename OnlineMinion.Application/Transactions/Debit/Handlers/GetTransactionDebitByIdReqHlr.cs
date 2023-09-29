using JetBrains.Annotations;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionDebitByIdReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetModelByIdReqHlr<GetTransactionDebitByIdReq, TransactionDebit, TransactionDebitResp>(dbContext)
{
    protected override TransactionDebitResp ToResponse(TransactionDebit entity) => new(
        entity.Id,
        entity.PaymentInstrumentId,
        entity.AccountSpecId,
        entity.Fee,
        entity.Date,
        entity.Amount,
        entity.Subject,
        entity.Party,
        entity.Tags
    );
}
