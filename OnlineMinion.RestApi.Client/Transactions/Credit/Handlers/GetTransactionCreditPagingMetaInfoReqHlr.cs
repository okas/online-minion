using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Credit.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.Transactions.Credit.Handlers;

[UsedImplicitly]
internal sealed class GetTransactionCreditPagingMetaInfoReqHlr(ApiProvider api)
    : BaseGetModelPagingMetaInfoReqHlr<GetTransactionCreditPagingMetaInfoReq>(
        api.Client,
        ApiProvider.ApiTransactionsCreditUri
    );
