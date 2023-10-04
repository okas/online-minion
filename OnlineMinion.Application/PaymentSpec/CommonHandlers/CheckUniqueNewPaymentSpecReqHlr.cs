using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.CommonHandlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewPaymentSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCheckUniqueModelReqHlr<CheckPaymentSpecUniqueNewReq, PaymentSpecCash>(dbContext)
{
    protected override Expression<Func<PaymentSpecCash, bool>> GetConflictPredicate(CheckPaymentSpecUniqueNewReq rq) =>
        entity => entity.Name == rq.MemberValue;
}
