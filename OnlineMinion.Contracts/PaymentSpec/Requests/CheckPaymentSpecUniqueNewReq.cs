using MediatR;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public record CheckPaymentSpecUniqueNewReq(string Name) : IRequest<bool>;
