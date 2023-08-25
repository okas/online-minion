using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CreateAccountSpecReqHlr : BaseCreateModelReqHlr<CreateAccountSpecReq>
{
    private readonly Uri _resource;

    public CreateAccountSpecReqHlr(ApiClientProvider api, ILogger<CreateAccountSpecReqHlr> logger)
        : base(api.Client, logger) =>
        _resource = api.ApiV1AccountSpecsUri;

    protected override string ModelName => "Account Specification";

    protected override Uri BuildUri() => _resource;
}
