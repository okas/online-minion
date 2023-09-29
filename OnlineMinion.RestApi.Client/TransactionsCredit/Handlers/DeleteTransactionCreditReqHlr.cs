using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.TransactionsCredit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionCreditReqHlr(ApiProvider api)
    : BaseDeleteModelReqHlr<DeleteTransactionCreditReq>(api.Client, ApiProvider.ApiTransactionsCreditUri);
