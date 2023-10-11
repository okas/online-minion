using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecBankHandlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecBankReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreatePaymentSpecBankReq, PaymentSpecBank>(dbContext)
{
    protected override PaymentSpecBank ToEntity(CreatePaymentSpecBankReq rq) => new(
        rq.IBAN,
        rq.BankName,
        rq.Name,
        rq.CurrencyCode,
        rq.Tags
    );
}
