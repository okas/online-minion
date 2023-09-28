using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionDebitReqHlr(IOnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeleteTransactionDebitReq, TransactionDebit>(dbContext);
