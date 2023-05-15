using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Web.AppMessaging.Requests;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal sealed class CreateAccountSpecReqHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<CreateAccountSpecReq, bool>
{
    private readonly HttpClient _httpClient;

    public CreateAccountSpecReqHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<bool> Handle(CreateAccountSpecReq request, CancellationToken cancellationToken)
    {
        using var message = await _httpClient.PostAsJsonAsync(UriApiV1AccountSpecs, request, cancellationToken);

        return message.IsSuccessStatusCode;
    }
}
