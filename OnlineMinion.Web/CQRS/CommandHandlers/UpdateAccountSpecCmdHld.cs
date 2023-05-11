using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Web.CQRS.Commands;
using OnlineMinion.Web.HttpClients;

namespace OnlineMinion.Web.CQRS.CommandHandlers;

internal sealed class UpdateAccountSpecCmdHld : BaseAccountSpecsRequestHandler,
    IRequestHandler<UpdateAccountSpecCmd, bool>
{
    private readonly ApiHttpClient _apiHttpClient;
    public UpdateAccountSpecCmdHld(ApiHttpClient apiHttpClient) => _apiHttpClient = apiHttpClient;

    public async Task<bool> Handle(UpdateAccountSpecCmd request, CancellationToken cancellationToken)
    {
        using var message = await _apiHttpClient.Client.PutAsJsonAsync(
            $"{UriApiV1AccountSpecs}/{request.Id}",
            request,
            cancellationToken
        );

        return message.IsSuccessStatusCode;
    }
}
