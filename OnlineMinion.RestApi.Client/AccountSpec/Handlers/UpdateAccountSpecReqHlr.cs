using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdateAccountSpecReqHlr(ApiProvider api, ILogger<UpdateAccountSpecReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdateAccountSpecReq>(api.Client, ApiProvider.ApiAccountSpecsUri, logger)
{
    protected override string ModelName => "Account Specification";
}
