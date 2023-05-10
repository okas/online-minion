using MediatR;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Web.CQRS.Queries;
using OnlineMinion.Web.HttpClients;

namespace OnlineMinion.Web.CQRS.QueryHandlers;

internal sealed class GetPagesCountBySizeQryHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecsPagesCountBySizeQry, int?>
{
    private readonly ApiHttpClient _apiHttpClient;
    public GetPagesCountBySizeQryHlr(ApiHttpClient apiHttpClient) => _apiHttpClient = apiHttpClient;

    public async Task<int?> Handle(GetAccountSpecsPagesCountBySizeQry request, CancellationToken cancellationToken)
    {
        using var result = await _apiHttpClient.Client.SendAsync(
            new(HttpMethod.Head, UriApiV1AccountSpecs + $"?page-size={request.Size}"),
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );

        return result.Headers.TryGetValues(CustomHeaderNames.PagingPages, out var values) &&
               int.TryParse(values.First(), out var pages)
            ? pages
            : null;
    }
}
