using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.AccountSpec.Requests;

public readonly record struct GetAccountSpecPagingMetaInfoReq(int PageSize = 10) : IGetPagingInfoRequest;
