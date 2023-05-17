using System.Diagnostics.CodeAnalysis;
using MediatR;
using OnlineMinion.Common.Utilities.Extensions;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Web.AppMessaging.Requests;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.AppMessaging.Handlers;

[SuppressMessage("Usage", "MA0011:IFormatProvider is missing")]
internal sealed class GetAccountSpecPageCountBySizeReqHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecPageCountBySizeReq, int?>
{
    private readonly HttpClient _httpClient;

    public GetAccountSpecPageCountBySizeReqHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<int?> Handle(GetAccountSpecPageCountBySizeReq request, CancellationToken ct)
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Head,
            UriApiV1AccountSpecs.AddQueryString(
                new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase)
                    { [nameof(request.PageSize)] = request.PageSize, }
            )
        );

        using var result = await _httpClient
            .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        return result.Headers.TryGetValues(CustomHeaderNames.PagingPages, out var values) &&
               int.TryParse(values.First(), out var pages)
            ? pages
            : null;
    }
}
