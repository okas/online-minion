using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Web.AppMessaging.Requests;
using OnlineMinion.Web.Infrastructure;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class CreateAccountSpecReqHlr : IRequestHandler<CreateAccountSpecReq, bool>
{
    private readonly ApiService _api;

    public CreateAccountSpecReqHlr(ApiService api) => _api = api;

    public async Task<bool> Handle(CreateAccountSpecReq request, CancellationToken cancellationToken)
    {
        using var message = await _api.Client.PostAsJsonAsync(_api.ApiV1AccountSpecsUri, request, cancellationToken)
            .ConfigureAwait(false);

        return message.IsSuccessStatusCode;
    }
}
