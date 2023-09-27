using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.DataStore;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionCreditReqHlr(OnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeleteTransactionCreditReq, TransactionCredit>(dbContext);
