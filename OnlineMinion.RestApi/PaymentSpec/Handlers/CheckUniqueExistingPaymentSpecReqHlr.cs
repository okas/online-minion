using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingPaymentSpecReqHlr
    : BaseCheckUniqueModelReqHlr<CheckPaymentSpecUniqueExistingReq, BasePaymentSpec>
{
    public CheckUniqueExistingPaymentSpecReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    protected override Expression<Func<BasePaymentSpec, bool>> GetConflictPredicate(
        CheckPaymentSpecUniqueExistingReq rq
    ) =>
        entity => entity.Name == rq.Name && entity.Id != rq.ExceptId;
}
