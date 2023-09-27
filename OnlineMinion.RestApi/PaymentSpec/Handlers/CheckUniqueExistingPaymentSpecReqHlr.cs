using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.DataStore;
using OnlineMinion.Domain.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingPaymentSpecReqHlr(OnlineMinionDbContext dbContext)
    : BaseCheckUniqueModelReqHlr<CheckPaymentSpecUniqueExistingReq, BasePaymentSpec>(dbContext)
{
    protected override Expression<Func<BasePaymentSpec, bool>> GetConflictPredicate(
        CheckPaymentSpecUniqueExistingReq rq
    ) =>
        entity => entity.Name == rq.MemberValue && entity.Id != rq.OwnId;
}
