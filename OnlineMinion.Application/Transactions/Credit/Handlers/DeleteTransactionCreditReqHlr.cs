using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionCreditReqHlr(IOnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeleteTransactionCreditReq, TransactionCredit>(dbContext);
