using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.AccountSpec.Requests;

public readonly record struct GetAccountSpecPageCountBySizeReq(int PageSize) : IGetPagingInfoReq;
