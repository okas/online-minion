using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewPaymentSpecReqHlr
    : BaseCheckUniqueModelReqHlr<CheckPaymentSpecUniqueNewReq, BasePaymentSpec>
{
    public CheckUniqueNewPaymentSpecReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    protected override Expression<Func<BasePaymentSpec, bool>> GetConflictPredicate(CheckPaymentSpecUniqueNewReq rq) =>
        entity => entity.Name == rq.Name;
}
