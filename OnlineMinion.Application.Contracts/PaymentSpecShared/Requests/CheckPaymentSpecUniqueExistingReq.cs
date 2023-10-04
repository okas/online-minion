using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;

public record CheckPaymentSpecUniqueExistingReq(string MemberValue, Guid OwnId) :
    ICheckUniqueExistingModelByMemberRequest;
