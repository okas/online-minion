using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Application.Contracts.Transactions.Debit.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.TransactionDebits;

namespace OnlineMinion.Application.TransactionsDebit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionDebitByIdReqHlr(IOnlineMinionDbContext dbContext) : BaseGetModelByIdReqHlr<
    GetTransactionDebitByIdReq,
    TransactionDebit,
    TransactionDebitId,
    TransactionDebitResp
>(dbContext)
{
    protected override TransactionDebitId CreateEntityId(GetTransactionDebitByIdReq rq) => new(rq.Id);

    protected override TransactionDebitResp ToResponse(TransactionDebit entity) => new(
        entity.Id.Value,
        entity.PaymentInstrumentId.Value,
        entity.AccountSpecId.Value,
        entity.Fee,
        entity.Date,
        entity.Amount,
        entity.Subject,
        entity.Party,
        entity.Tags
    );
}
