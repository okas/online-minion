using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;

public record CheckUniqueNewPaymentSpecNameReq(string MemberValue) : ICheckUniqueNewModelByMemberRequest;
