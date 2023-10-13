using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;

public record CheckUniqueNewPaymentSpecBankIBANReq(string MemberValue) : ICheckUniqueNewModelByMemberRequest;
