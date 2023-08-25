using JetBrains.Annotations;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPagedPaymentSpecsReqHlr : BaseGetSomeModelsPagedReqHlr<PaymentSpecResp>
{
    private readonly Uri _resource;

    public GetPagedPaymentSpecsReqHlr(ApiClientProvider api) : base(api.Client) =>
        _resource = api.ApiV1PaymentSpecsUri;

    protected override Uri BuildUrl(IQueryParams rq) => AddQueryString(_resource, rq);
}
