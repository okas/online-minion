using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.Common.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckPaymentSpecUniqueNewReqHlr
    : BaseUniquenessCheckReqHlr<CheckPaymentSpecUniqueNewReq, BasePaymentSpec>
{
    public CheckPaymentSpecUniqueNewReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    protected override Expression<Func<BasePaymentSpec, bool>> GetConflictPredicate(CheckPaymentSpecUniqueNewReq rq) =>
        entity => entity.Name == rq.Name;
}
