using MediatR;

namespace OnlineMinion.Web.CQRS.Queries;

internal readonly record struct GetAccountSpecPageCountBySizeQry(int PageSize) : IRequest<int?>;
