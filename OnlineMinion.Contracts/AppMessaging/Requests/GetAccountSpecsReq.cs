using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public record GetAccountSpecsReq(int Page = 1, int PageSize = 10)
    : IRequest<PagedResult<AccountSpecResp>>, IPagedRequest;
