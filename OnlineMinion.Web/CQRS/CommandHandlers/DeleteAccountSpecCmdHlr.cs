using MediatR;
using OnlineMinion.Contracts.Commands;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.CQRS.CommandHandlers;

internal sealed class DeleteAccountSpecCmdHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<DeleteAccountSpecCmd, bool>
{
    private readonly HttpClient _httpClient;

    public DeleteAccountSpecCmdHlr(IHttpClientFactory factory) =>
        _httpClient = factory.CreateClient(Constants.ApiClient);

    public async Task<bool> Handle(DeleteAccountSpecCmd request, CancellationToken cancellationToken)
    {
        var uri = $"{UriApiV1AccountSpecs}/{request.Id}";
        var responseMessage = await _httpClient.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);

        return responseMessage.IsSuccessStatusCode;
    }
}
