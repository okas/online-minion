using JetBrains.Annotations;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecPageCountBySizeReqHlr
    : BaseGetModelPageCountReqHlr<GetPaymentSpecPageCountBySizeReq>
{
    private readonly Uri _resource;

    public GetPaymentSpecPageCountBySizeReqHlr(ApiClientProvider api) : base(api.Client) =>
        _resource = api.ApiV1PaymentSpecsUri;

    protected override Uri BuildUrl(GetPaymentSpecPageCountBySizeReq rq) => AddQueryString(_resource, rq);
}
