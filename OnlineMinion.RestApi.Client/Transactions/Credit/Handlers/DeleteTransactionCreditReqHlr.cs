using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class DeleteTransactionCreditReqHlr(ApiClientProvider api)
    : BaseDeleteModelReqHlr<DeleteTransactionCreditReq>(api.Client, api.ApiV1TransactionsCreditUri);
