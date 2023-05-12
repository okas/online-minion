using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Contracts.Queries;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.CQRS.QueryHandlers;

internal sealed class GetAccountSpecByIdQryHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecByIdQry, AccountSpecResp?>
{
    private readonly HttpClient _httpClient;

    public GetAccountSpecByIdQryHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<AccountSpecResp?> Handle(GetAccountSpecByIdQry request, CancellationToken cancellationToken) =>
        await _httpClient.GetFromJsonAsync<AccountSpecResp?>($"{UriApiV1AccountSpecs}/{request.Id}", cancellationToken);
}
