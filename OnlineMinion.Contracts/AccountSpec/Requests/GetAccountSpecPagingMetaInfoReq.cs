using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public readonly record struct GetAccountSpecPagingMetaInfoReq(int PageSize = 10) : IGetPagingInfoRequest;
