using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class DeleteAccountSpecReqHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<DeleteAccountSpecReq, bool>
{
    private readonly HttpClient _httpClient;

    public DeleteAccountSpecReqHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<bool> Handle(DeleteAccountSpecReq request, CancellationToken cancellationToken)
    {
        var uri = $"{UriApiV1AccountSpecs}/{request.Id}";
        var responseMessage = await _httpClient.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);

        return responseMessage.IsSuccessStatusCode;
    }
}
