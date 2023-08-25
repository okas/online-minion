using OnlineMinion.Contracts.Common.Requests;

namespace OnlineMinion.RestApi.Client.AccountSpec.Requests;

public readonly record struct GetAccountSpecPageCountBySizeReq(int PageSize) : IGetPagingInfoReq;
