using MediatR;
using OnlineMinion.Common.Infrastructure.Extensions;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Web.CQRS.Queries;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.CQRS.QueryHandlers;

internal sealed class GetAccountSpecPageCountBySizeQryHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecPageCountBySizeQry, int?>
{
    private readonly HttpClient _httpClient;

    public GetAccountSpecPageCountBySizeQryHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<int?> Handle(GetAccountSpecPageCountBySizeQry request, CancellationToken cancellationToken)
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Head,
            UriApiV1AccountSpecs.AddQueryString(
                new Dictionary<string, object?> { [nameof(request.PageSize)] = request.PageSize }
            )
        );

        using var result = await _httpClient
            .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        return result.Headers.TryGetValues(CustomHeaderNames.PagingPages, out var values) &&
               int.TryParse(values.First(), out var pages)
            ? pages
            : null;
    }
}
