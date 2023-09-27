using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Contracts.Transactions.Debit.Responses;
using OnlineMinion.DataStore;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionDebitByIdReqHlr(OnlineMinionDbContext dbContext)
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
