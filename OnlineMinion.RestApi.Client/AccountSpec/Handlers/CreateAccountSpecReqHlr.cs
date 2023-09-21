using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CreateAccountSpecReqHlr(ApiProvider api, ILogger<CreateAccountSpecReqHlr> logger)
    : BaseCreateModelReqHlr<CreateAccountSpecReq>(api.Client, api.ApiAccountSpecsUri, logger)
{
    protected override string ModelName => "Account Specification";
}
