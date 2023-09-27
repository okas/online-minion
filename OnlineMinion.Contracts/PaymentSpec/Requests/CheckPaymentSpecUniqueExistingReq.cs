using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.Contracts.PaymentSpec.Requests;

public record CheckPaymentSpecUniqueExistingReq(string MemberValue, Guid OwnId) :
    ICheckUniqueExistingModelByMemberRequest;
