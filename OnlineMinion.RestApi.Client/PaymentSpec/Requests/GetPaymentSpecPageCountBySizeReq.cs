using MediatR;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Requests;

public readonly record struct GetPaymentSpecPageCountBySizeReq(int PageSize) : IRequest<int?>;
