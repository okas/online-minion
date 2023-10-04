using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;

public readonly record struct GetPaymentSpecPagingMetaInfoReq(int PageSize = 10) : IGetPagingInfoRequest;
