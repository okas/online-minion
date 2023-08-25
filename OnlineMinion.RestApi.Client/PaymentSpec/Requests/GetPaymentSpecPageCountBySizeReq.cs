using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Requests;

public readonly record struct GetPaymentSpecPageCountBySizeReq(int PageSize) : IGetPagingInfoReq;
