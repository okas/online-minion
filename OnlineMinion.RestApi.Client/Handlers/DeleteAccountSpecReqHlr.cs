using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class DeleteAccountSpecReqHlr : IRequestHandler<DeleteAccountSpecReq, bool>
{
    private readonly ApiClientProvider _api;
    public DeleteAccountSpecReqHlr(ApiClientProvider api) => _api = api;

    public async Task<bool> Handle(DeleteAccountSpecReq request, CancellationToken cancellationToken)
    {
        var uri = $"{_api.ApiV1AccountSpecsUri}/{request.Id}";
        var responseMessage = await _api.Client.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);

        return responseMessage.IsSuccessStatusCode;
    }
}
