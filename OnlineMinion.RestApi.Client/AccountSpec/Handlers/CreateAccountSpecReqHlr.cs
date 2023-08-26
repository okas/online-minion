using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CreateAccountSpecReqHlr(ApiClientProvider api, ILogger<CreateAccountSpecReqHlr> logger)
    : BaseCreateModelReqHlr<CreateAccountSpecReq>(api.Client, api.ApiV1AccountSpecsUri, logger)
{
    protected override string ModelName => "Account Specification";
}
