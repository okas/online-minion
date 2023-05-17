using MediatR;

namespace OnlineMinion.Web.AppMessaging.Requests;

internal readonly record struct GetAccountSpecPageCountBySizeReq(int PageSize) : IRequest<int?>;
