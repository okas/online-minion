using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Web.CQRS.Commands;
using OnlineMinion.Web.HttpClients;

namespace OnlineMinion.Web.CQRS.CommandHandlers;

internal sealed class CreateAccountSpecCmdHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<CreateAccountSpecCmd, bool>
{
    private readonly ApiHttpClient _apiHttpClient;
    public CreateAccountSpecCmdHlr(ApiHttpClient apiHttpClient) => _apiHttpClient = apiHttpClient;

    public async Task<bool> Handle(CreateAccountSpecCmd request, CancellationToken cancellationToken)
    {
        using var message = await _apiHttpClient.Client.PostAsJsonAsync(
            UriApiV1AccountSpecs,
            request,
            cancellationToken
        );

        return message.IsSuccessStatusCode;
    }
}
