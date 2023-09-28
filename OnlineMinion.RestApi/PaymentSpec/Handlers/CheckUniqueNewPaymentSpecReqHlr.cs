using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Domain.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewPaymentSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCheckUniqueModelReqHlr<CheckPaymentSpecUniqueNewReq, BasePaymentSpec>(dbContext)
{
    protected override Expression<Func<BasePaymentSpec, bool>> GetConflictPredicate(CheckPaymentSpecUniqueNewReq rq) =>
        entity => entity.Name == rq.MemberValue;
}
