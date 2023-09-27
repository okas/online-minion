using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit.Requests;
using OnlineMinion.DataStore;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionDebitReqHlr(OnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeleteTransactionDebitReq, TransactionDebit>(dbContext);
