using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Credit.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class GetPagedTransactionCreditsReqHlr(ApiProvider api)
    : BaseGetSomeModelsPagedReqHlr<TransactionCreditResp>(api.Client, api.ApiTransactionsCreditUri);
