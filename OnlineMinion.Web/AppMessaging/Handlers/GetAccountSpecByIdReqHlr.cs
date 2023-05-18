using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Web.Infrastructure;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class GetAccountSpecByIdReqHlr : IRequestHandler<GetAccountSpecByIdReq, AccountSpecResp?>
{
    private readonly ApiService _api;
    public GetAccountSpecByIdReqHlr(ApiService api) => _api = api;

    public async Task<AccountSpecResp?> Handle(GetAccountSpecByIdReq request, CancellationToken cancellationToken)
    {
        var uri = $"{_api.ApiV1AccountSpecsUri}/{request.Id}";

        return await _api.Client.GetFromJsonAsync<AccountSpecResp?>(uri, cancellationToken).ConfigureAwait(false);
    }
}
