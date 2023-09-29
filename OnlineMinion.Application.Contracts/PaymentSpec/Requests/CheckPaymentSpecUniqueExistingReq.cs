using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpec.Requests;

public record CheckPaymentSpecUniqueExistingReq(string MemberValue, Guid OwnId) :
    ICheckUniqueExistingModelByMemberRequest;
