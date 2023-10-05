using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecDescriptorsReqHlr(ApiProvider api)
    : GetSomeModelDescriptorsReqHlr<AccountSpecDescriptorResp>(api.Client, ApiAccountSpecsUri);
