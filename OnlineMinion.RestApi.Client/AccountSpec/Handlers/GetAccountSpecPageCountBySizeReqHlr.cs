using JetBrains.Annotations;
using OnlineMinion.RestApi.Client.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecPageCountBySizeReqHlr
    : BaseGetModelPageCountReqHlr<GetAccountSpecPageCountBySizeReq>
{
    private readonly Uri _resource;

    public GetAccountSpecPageCountBySizeReqHlr(ApiClientProvider api) : base(api.Client) =>
        _resource = api.ApiV1AccountSpecsUri;

    protected override Uri BuildUrl(GetAccountSpecPageCountBySizeReq rq) => AddQueryString(_resource, rq);
}
