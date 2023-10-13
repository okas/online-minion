using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;

public record CheckUniqueExistingPaymentSpecBankIBANReq(string MemberValue, Guid OwnId)
    : ICheckUniqueExistingModelByMemberRequest;
