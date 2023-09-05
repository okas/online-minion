using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecDescriptorsReqHlr(ApiClientProvider api)
    : GetSomeModelDescriptorsReqHlr<AccountSpecDescriptorResp>(api.Client, api.ApiV1AccountSpecsUri);