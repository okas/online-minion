using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Contracts.Queries;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Web.HttpClients;

namespace OnlineMinion.Web.CQRS.QueryHandlers;

internal sealed class GetPagedAccountSpecsQryHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecsQry, BasePagedResult<AccountSpecResp>>
{
    private readonly ApiHttpClient _apiHttpClient;
    public GetPagedAccountSpecsQryHlr(ApiHttpClient apiHttpClient) => _apiHttpClient = apiHttpClient;

    public async Task<BasePagedResult<AccountSpecResp>> Handle(
        GetAccountSpecsQry request,
        CancellationToken  cancellationToken
    ) => await _apiHttpClient.Client.GetFromJsonAsync<BasePagedResult<AccountSpecResp>>(
        UriApiV1AccountSpecs + $"?{nameof(request.Page)}={request.Page}&{nameof(request.PageSize)}={request.PageSize}",
        cancellationToken
    );
}
