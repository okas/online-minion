using System.Net.Http.Json;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
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
