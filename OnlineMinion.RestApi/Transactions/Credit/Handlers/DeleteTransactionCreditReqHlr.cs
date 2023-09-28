using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionCreditReqHlr(IOnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeleteTransactionCreditReq, TransactionCredit>(dbContext);
