using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecBankHandlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecBankReqHlr(IOnlineMinionDbContext dbContext)
    : BaseUpdateModelReqHlr<UpdatePaymentSpecBankReq, PaymentSpecBank, PaymentSpecId>(dbContext)
{
    protected override PaymentSpecId CreateEntityId(UpdatePaymentSpecBankReq rq) => new(rq.Id);

    protected override void UpdateEntity(PaymentSpecBank entity, UpdatePaymentSpecBankReq rq) =>
        entity.Update(rq.IBAN, rq.BankName, rq.Name, rq.Tags);
}
