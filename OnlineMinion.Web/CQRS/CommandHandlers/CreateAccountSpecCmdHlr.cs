using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Web.CQRS.Commands;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.CQRS.CommandHandlers;

internal sealed class CreateAccountSpecCmdHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<CreateAccountSpecCmd, bool>
{
    private readonly HttpClient _httpClient;

    public CreateAccountSpecCmdHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<bool> Handle(CreateAccountSpecCmd request, CancellationToken cancellationToken)
    {
        using var message = await _httpClient.PostAsJsonAsync(UriApiV1AccountSpecs, request, cancellationToken);

        return message.IsSuccessStatusCode;
    }
}
