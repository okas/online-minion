using System.Net.Http.Json;
using MediatR;
using OnlineMinion.Contracts.Queries;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Web.HttpClients;

namespace OnlineMinion.Web.CQRS.QueryHandlers;

internal sealed class GetPagedAccountSpecsQryHlr : BaseAccountSpecsRequestHandler,
    IRequestHandler<GetAccountSpecsQry, BasePagedResult<AccountSpecResp>>
{
    private readonly Api _api;
    public GetPagedAccountSpecsQryHlr(Api api) => _api = api;

    public async Task<BasePagedResult<AccountSpecResp>> Handle(
        GetAccountSpecsQry request,
        CancellationToken  cancellationToken
    ) => await _api.Client.GetFromJsonAsync<BasePagedResult<AccountSpecResp>>(
        UriApiV1AccountSpecs + $"?{nameof(request.Page)}={request.Page}&{nameof(request.PageSize)}={request.PageSize}",
        cancellationToken
    );
}
