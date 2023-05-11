using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Contracts.Queries;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Web.HttpClients;

namespace OnlineMinion.Web.CQRS.QueryHandlers;

internal sealed class GetAccountSpecByIdQryHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecByIdQry, AccountSpecResp?>
{
    private readonly ApiHttpClient _apiHttpClient;
    public GetAccountSpecByIdQryHlr(ApiHttpClient apiHttpClient) => _apiHttpClient = apiHttpClient;

    public async Task<AccountSpecResp?> Handle(GetAccountSpecByIdQry request, CancellationToken cancellationToken) =>
        await _apiHttpClient.Client.GetFromJsonAsync<AccountSpecResp?>(
            $"{UriApiV1AccountSpecs}/{request.Id}",
            cancellationToken
        );
}
