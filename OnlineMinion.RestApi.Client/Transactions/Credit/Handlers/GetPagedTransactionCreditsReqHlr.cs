using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Responses;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class GetPagedTransactionCreditsReqHlr(ApiClientProvider api)
    : BasePagedGetSomeModelsReqHlr<TransactionCreditResp>(api.Client, api.ApiV1TransactionsCreditUri);
