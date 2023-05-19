using System.Net.Http.Json;
using MediatR;
using OnlineMinion.RestApi.Client.Infrastructure;
using OnlineMinion.RestApi.Client.Requests;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class CreateAccountSpecReqHlr : IRequestHandler<CreateAccountSpecReq, bool>
{
    private readonly ApiClientProvider _api;

    public CreateAccountSpecReqHlr(ApiClientProvider api) => _api = api;

    public async Task<bool> Handle(CreateAccountSpecReq request, CancellationToken cancellationToken)
    {
        using var message = await _api.Client.PostAsJsonAsync(_api.ApiV1AccountSpecsUri, request, cancellationToken)
            .ConfigureAwait(false);

        return message.IsSuccessStatusCode;
    }
}
