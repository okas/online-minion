using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.TransactionDebits;

namespace OnlineMinion.Application.TransactionsDebit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionDebitReqHlr(IOnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeleteTransactionDebitReq, TransactionDebit, TransactionDebitId>(dbContext)
{
    protected override TransactionDebitId CreateEntityId(DeleteTransactionDebitReq rq) => new(rq.Id);
}
