using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Common.Utilities.Extensions;
using OnlineMinion.Contracts.Queries;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.CQRS.QueryHandlers;

internal sealed class GetPagedAccountSpecsQryHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecsQry, BasePagedResult<AccountSpecResp>>
{
    private readonly HttpClient _httpClient;

    public GetPagedAccountSpecsQryHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<BasePagedResult<AccountSpecResp>> Handle(GetAccountSpecsQry request, CancellationToken ct)
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
