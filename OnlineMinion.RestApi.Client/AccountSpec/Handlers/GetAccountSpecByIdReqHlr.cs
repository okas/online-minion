using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecByIdReqHlr(ApiClientProvider api)
    : BaseGetModelByIdReqHlr<GetAccountSpecByIdReq, AccountSpecResp?>(api.Client, api.ApiV1AccountSpecsUri);
