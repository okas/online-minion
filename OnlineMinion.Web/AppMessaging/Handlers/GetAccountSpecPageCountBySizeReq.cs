using MediatR;

namespace OnlineMinion.Web.AppMessaging.Handlers;

internal readonly record struct GetAccountSpecPageCountBySizeReq(int PageSize) : IRequest<int?>;
