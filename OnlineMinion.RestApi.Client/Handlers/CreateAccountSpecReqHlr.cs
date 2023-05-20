using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class CreateAccountSpecReqHlr : IRequestHandler<CreateAccountSpecReq, ModelIdResp?>
{
    private readonly ApiClientProvider _api;

    public CreateAccountSpecReqHlr(ApiClientProvider api) => _api = api;

    public async Task<ModelIdResp?> Handle(CreateAccountSpecReq request, CancellationToken cancellationToken)
    {
        using var message = await _api.Client.PostAsJsonAsync(_api.ApiV1AccountSpecsUri, request, cancellationToken)
            .ConfigureAwait(false);

        return await message.Content.ReadFromJsonAsync<ModelIdResp>(cancellationToken).ConfigureAwait(false);
    }
}
