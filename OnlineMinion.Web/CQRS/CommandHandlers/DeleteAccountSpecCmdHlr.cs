using MediatR;
using OnlineMinion.Contracts.Commands;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.CQRS.CommandHandlers;

internal sealed class DeleteAccountSpecCmdHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<DeleteAccountSpecCmd, int>
{
    private readonly HttpClient _httpClient;

    public DeleteAccountSpecCmdHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);


    public async Task<int> Handle(DeleteAccountSpecCmd request, CancellationToken cancellationToken) =>
        (await _httpClient.DeleteAsync($"{UriApiV1AccountSpecs}/{request.Id}", cancellationToken)).IsSuccessStatusCode
            ? 1
            : 0;
}
