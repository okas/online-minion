using JetBrains.Annotations;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPagedAccountSpecsReqHlr : BaseGetSomeModelsPagedReqHlr<AccountSpecResp>
{
    private readonly Uri _resource;

    public GetPagedAccountSpecsReqHlr(ApiClientProvider api) : base(api.Client) =>
        _resource = api.ApiV1AccountSpecsUri;

    protected override Uri BuildUrl(IQueryParams rq) => AddQueryString(_resource, rq);
}
