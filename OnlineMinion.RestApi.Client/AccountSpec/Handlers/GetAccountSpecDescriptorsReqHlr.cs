using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecDescriptorsReqHlr(ApiProvider api)
    : GetSomeModelDescriptorsReqHlr<AccountSpecDescriptorResp>(api.Client, ApiProvider.ApiAccountSpecsUri);
