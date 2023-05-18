using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Web.Infrastructure;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class DeleteAccountSpecReqHlr : IRequestHandler<DeleteAccountSpecReq, bool>
{
    private readonly ApiService _api;
    public DeleteAccountSpecReqHlr(ApiService api) => _api = api;

    public async Task<bool> Handle(DeleteAccountSpecReq request, CancellationToken cancellationToken)
    {
        var uri = $"{_api.ApiV1AccountSpecsUri}/{request.Id}";
        var responseMessage = await _api.Client.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);

        return responseMessage.IsSuccessStatusCode;
    }
}
