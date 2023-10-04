using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.CommonHandlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingPaymentSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCheckUniqueModelReqHlr<CheckPaymentSpecUniqueExistingReq, PaymentSpecCash>(dbContext)
{
    protected override Expression<Func<PaymentSpecCash, bool>> GetConflictPredicate(
        CheckPaymentSpecUniqueExistingReq rq
    ) =>
        entity => entity.Name == rq.MemberValue
                  && entity.Id != new PaymentSpecId(rq.OwnId);
}
