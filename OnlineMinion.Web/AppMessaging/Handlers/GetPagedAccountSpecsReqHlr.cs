using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Common.Utilities.Extensions;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class GetPagedAccountSpecsReqHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecsReq, BasePagedResult<AccountSpecResp>>
{
    private readonly HttpClient _httpClient;

    public GetPagedAccountSpecsReqHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<BasePagedResult<AccountSpecResp>> Handle(GetAccountSpecsReq request, CancellationToken ct)
    {
        var uri = UriApiV1AccountSpecs.AddQueryString(
            new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase)
            {
                [nameof(request.Page)] = request.Page,
                [nameof(request.PageSize)] = request.PageSize,
            }
        );

        return await _httpClient.GetFromJsonAsync<BasePagedResult<AccountSpecResp>>(uri, ct).ConfigureAwait(false);
    }
}
