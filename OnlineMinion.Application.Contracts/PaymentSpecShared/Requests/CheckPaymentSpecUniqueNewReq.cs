using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;

public record CheckPaymentSpecUniqueNewReq(string MemberValue) : ICheckUniqueNewModelByMemberRequest;
