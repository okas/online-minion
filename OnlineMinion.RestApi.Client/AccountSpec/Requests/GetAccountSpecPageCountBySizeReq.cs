using MediatR;

namespace OnlineMinion.RestApi.Client.AccountSpec.Requests;

public readonly record struct GetAccountSpecPageCountBySizeReq(int PageSize) : IRequest<int?>;
