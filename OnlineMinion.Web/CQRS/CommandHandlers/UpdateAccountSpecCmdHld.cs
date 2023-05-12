using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Web.CQRS.Commands;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.CQRS.CommandHandlers;

internal sealed class UpdateAccountSpecCmdHld : BaseAccountSpecsRequestHandler,
    IRequestHandler<UpdateAccountSpecCmd, bool>
{
    private readonly HttpClient _httpClient;

    public UpdateAccountSpecCmdHld(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<bool> Handle(UpdateAccountSpecCmd request, CancellationToken cancellationToken)
    {
        var uri = $"{UriApiV1AccountSpecs}/{request.Id}";
        using var message = await _httpClient.PutAsJsonAsync(uri, request, cancellationToken);

        return message.IsSuccessStatusCode;
    }
}
