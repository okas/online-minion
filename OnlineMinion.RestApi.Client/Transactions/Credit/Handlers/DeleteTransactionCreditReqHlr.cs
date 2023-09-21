using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionCreditReqHlr(ApiProvider api)
    : BaseDeleteModelReqHlr<DeleteTransactionCreditReq>(api.Client, api.ApiTransactionsCreditUri);
