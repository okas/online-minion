using MediatR;
using OnlineMinion.Common.Utilities.Extensions;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.RestApi.Client.Infrastructure;
using OnlineMinion.RestApi.Client.Requests;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class GetAccountSpecPageCountBySizeReqHlr :
    IRequestHandler<GetAccountSpecPageCountBySizeReq, int?>
{
    private readonly ApiClientProvider _api;
    public GetAccountSpecPageCountBySizeReqHlr(ApiClientProvider api) => _api = api;

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
