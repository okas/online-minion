using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public record CheckPaymentSpecUniqueExistingReq(string Name, int ExceptId) : IRequest<ErrorOr<Success>>;
