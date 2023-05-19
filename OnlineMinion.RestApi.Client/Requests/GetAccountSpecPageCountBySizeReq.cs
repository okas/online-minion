using MediatR;

namespace OnlineMinion.RestApi.Client.Requests;

public readonly record struct GetAccountSpecPageCountBySizeReq(int PageSize) : IRequest<int?>;
