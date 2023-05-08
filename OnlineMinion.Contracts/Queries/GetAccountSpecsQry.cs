using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.Queries;

public record GetAccountSpecsQry(int Page = 1, int PageSize = 10) : IRequest<BasePagedResult<AccountSpecResp>>;
