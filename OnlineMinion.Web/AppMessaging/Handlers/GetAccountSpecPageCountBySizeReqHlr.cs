using MediatR;
using OnlineMinion.Common.Utilities.Extensions;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Web.AppMessaging.Requests;
using OnlineMinion.Web.Infrastructure;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class GetAccountSpecPageCountBySizeReqHlr :
    IRequestHandler<GetAccountSpecPageCountBySizeReq, int?>
{
    private readonly ApiService _api;
    public GetAccountSpecPageCountBySizeReqHlr(ApiService api) => _api = api;

    public async Task<int?> Handle(GetAccountSpecPageCountBySizeReq request, CancellationToken ct)
    {
        var uri = _api.ApiV1AccountSpecsUri.AddQueryString(
            new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase)
                { [nameof(request.PageSize)] = request.PageSize, }
        );

        var message = new HttpRequestMessage(HttpMethod.Head, uri);

        using var result = await _api.Client
            .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        return result.Headers.GetHeaderFirstValue<int?>(CustomHeaderNames.PagingPages);
    }
}
