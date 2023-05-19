using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class GetAccountSpecByIdReqHlr : IRequestHandler<GetAccountSpecByIdReq, AccountSpecResp?>
{
    private readonly ApiClientProvider _api;
    public GetAccountSpecByIdReqHlr(ApiClientProvider api) => _api = api;

    public async Task<AccountSpecResp?> Handle(GetAccountSpecByIdReq request, CancellationToken cancellationToken)
    {
        var uri = $"{_api.ApiV1AccountSpecsUri}/{request.Id}";

        return await _api.Client.GetFromJsonAsync<AccountSpecResp?>(uri, cancellationToken).ConfigureAwait(false);
    }
}
