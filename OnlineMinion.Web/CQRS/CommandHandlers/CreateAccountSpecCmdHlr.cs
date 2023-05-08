using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Web.CQRS.Commands;
using OnlineMinion.Web.CQRS.QueryHandlers;
using OnlineMinion.Web.HttpClients;

namespace OnlineMinion.Web.CQRS.CommandHandlers;

internal sealed class CreateAccountSpecCmdHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<CreateAccountSpecCmd, bool>
{
    private readonly Api _api;
    public CreateAccountSpecCmdHlr(Api api) => _api = api;

    public async Task<bool> Handle(CreateAccountSpecCmd request, CancellationToken cancellationToken)
    {
        using var message = await _api.Client.PostAsJsonAsync(UriApiV1AccountSpecs, request, cancellationToken);

        return message.IsSuccessStatusCode;
    }
}
