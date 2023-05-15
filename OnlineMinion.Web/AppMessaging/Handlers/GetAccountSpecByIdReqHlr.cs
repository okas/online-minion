using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class GetAccountSpecByIdReqHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecByIdReq, AccountSpecResp?>
{
    private readonly HttpClient _httpClient;

    public GetAccountSpecByIdReqHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<AccountSpecResp?> Handle(GetAccountSpecByIdReq request, CancellationToken cancellationToken) =>
        await _httpClient.GetFromJsonAsync<AccountSpecResp?>($"{UriApiV1AccountSpecs}/{request.Id}", cancellationToken);
}
