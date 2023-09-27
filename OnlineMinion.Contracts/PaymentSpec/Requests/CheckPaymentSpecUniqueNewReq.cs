using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public record CheckPaymentSpecUniqueNewReq(string MemberValue) : ICheckUniqueNewModelByMemberRequest;
