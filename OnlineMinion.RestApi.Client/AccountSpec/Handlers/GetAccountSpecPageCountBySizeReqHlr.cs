using JetBrains.Annotations;
using OnlineMinion.RestApi.Client.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecPageCountBySizeReqHlr(ApiClientProvider api)
    : BaseGetModelPageCountReqHlr<GetAccountSpecPageCountBySizeReq>(api.Client, api.ApiV1AccountSpecsUri);
