using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public readonly record struct GetPaymentSpecPagingMetaInfoReq(int PageSize = 10) : IGetPagingInfoRequest;
