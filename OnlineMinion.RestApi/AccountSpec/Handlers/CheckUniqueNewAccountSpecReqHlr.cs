using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Common.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewAccountSpecReqHlr
    : BaseCheckUniqueModelReqHlr<CheckAccountSpecUniqueNewReq, Data.Entities.AccountSpec>
{
    public CheckUniqueNewAccountSpecReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    protected override Expression<Func<Data.Entities.AccountSpec, bool>> GetConflictPredicate(
        CheckAccountSpecUniqueNewReq rq
    ) =>
        entity => entity.Name == rq.Name;
}
