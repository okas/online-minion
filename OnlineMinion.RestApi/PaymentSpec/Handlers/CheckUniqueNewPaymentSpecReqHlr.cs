using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.DataStore;
using OnlineMinion.Domain.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewPaymentSpecReqHlr(OnlineMinionDbContext dbContext)
    : BaseCheckUniqueModelReqHlr<CheckPaymentSpecUniqueNewReq, BasePaymentSpec>(dbContext)
{
    protected override Expression<Func<BasePaymentSpec, bool>> GetConflictPredicate(CheckPaymentSpecUniqueNewReq rq) =>
        entity => entity.Name == rq.MemberValue;
}
