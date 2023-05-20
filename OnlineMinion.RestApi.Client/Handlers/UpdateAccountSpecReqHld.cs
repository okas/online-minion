using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class UpdateAccountSpecReqHld : IRequestHandler<UpdateAccountSpecReq, bool>
{
    private readonly ApiClientProvider _api;
    public UpdateAccountSpecReqHld(ApiClientProvider api) => _api = api;

    public async Task<bool> Handle(UpdateAccountSpecReq request, CancellationToken cancellationToken)
    {
        var uri = $"{_api.ApiV1AccountSpecsUri}/{request.Id}";
        using var message = await _api.Client.PutAsJsonAsync(uri, request, cancellationToken).ConfigureAwait(false);

        return message.IsSuccessStatusCode;
    }
}
