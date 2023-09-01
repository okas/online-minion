using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Debit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionDebitReqHlr(OnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeleteTransactionCreditReq, TransactionCredit>(dbContext);
