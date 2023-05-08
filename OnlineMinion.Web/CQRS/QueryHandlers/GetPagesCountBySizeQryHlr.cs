using MediatR;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Web.CQRS.Queries;
using OnlineMinion.Web.HttpClients;

namespace OnlineMinion.Web.CQRS.QueryHandlers;

internal sealed class GetPagesCountBySizeQryHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecsPagesCountBySizeQry, int?>
{
    private readonly Api _api;
    public GetPagesCountBySizeQryHlr(Api api) => _api = api;

    public async Task<int?> Handle(GetAccountSpecsPagesCountBySizeQry request, CancellationToken cancellationToken)
    {
        using var result = await _api.Client.SendAsync(
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
