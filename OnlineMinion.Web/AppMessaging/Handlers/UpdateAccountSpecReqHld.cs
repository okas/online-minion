using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Web.AppMessaging.Requests;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class UpdateAccountSpecReqHld : BaseAccountSpecsRequestHandler,
    IRequestHandler<UpdateAccountSpecReq, bool>
{
    private readonly HttpClient _httpClient;

    public UpdateAccountSpecReqHld(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<bool> Handle(UpdateAccountSpecReq request, CancellationToken cancellationToken)
    {
        var uri = $"{UriApiV1AccountSpecs}/{request.Id}";
        using var message = await _httpClient.PutAsJsonAsync(uri, request, cancellationToken);

        return message.IsSuccessStatusCode;
    }
}
