using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecPagingMetaInfoReqHlr(ApiClientProvider api)
    : BaseGetModelPagingMetaInfoReqHlr<GetAccountPagingMetaInfoReq>(api.Client, api.ApiV1AccountSpecsUri);
