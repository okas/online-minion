using MediatR;
using OnlineMinion.Common.Utilities.Extensions;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Web.AppMessaging.Requests;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class GetAccountSpecPageCountBySizeReqHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecPageCountBySizeReq, int?>
{
    private readonly HttpClient _httpClient;

    public GetAccountSpecPageCountBySizeReqHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<int?> Handle(GetAccountSpecPageCountBySizeReq request, CancellationToken ct)
    {
        var uri = UriApiV1AccountSpecs.AddQueryString(
            new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase)
                { [nameof(request.PageSize)] = request.PageSize, }
        );

        var message = new HttpRequestMessage(HttpMethod.Head, uri);

        using var result = await _httpClient
            .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        return result.Headers.GetHeaderFirstValue<int?>(CustomHeaderNames.PagingPages);
    }
}
