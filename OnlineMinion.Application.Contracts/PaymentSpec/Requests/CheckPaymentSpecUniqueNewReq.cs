using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpec.Requests;

public record CheckPaymentSpecUniqueNewReq(string MemberValue) : ICheckUniqueNewModelByMemberRequest;
