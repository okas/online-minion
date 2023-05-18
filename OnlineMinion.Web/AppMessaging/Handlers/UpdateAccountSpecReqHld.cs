using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Web.AppMessaging.Requests;
using OnlineMinion.Web.Infrastructure;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class UpdateAccountSpecReqHld : IRequestHandler<UpdateAccountSpecReq, bool>
{
    private readonly ApiService _api;
    public UpdateAccountSpecReqHld(ApiService api) => _api = api;

    public async Task<bool> Handle(UpdateAccountSpecReq request, CancellationToken cancellationToken)
    {
        var uri = $"{_api.ApiV1AccountSpecsUri}/{request.Id}";
        using var message = await _api.Client.PutAsJsonAsync(uri, request, cancellationToken).ConfigureAwait(false);

        return message.IsSuccessStatusCode;
    }
}
