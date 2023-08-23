using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Common.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckAccountSpecUniqueNewReqHlr
    : BaseUniquenessCheckReqHlr<CheckAccountSpecUniqueNewReq, Data.Entities.AccountSpec>
{
    public CheckAccountSpecUniqueNewReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    protected override Expression<Func<Data.Entities.AccountSpec, bool>> GetConflictPredicate(
        CheckAccountSpecUniqueNewReq rq
    ) =>
        entity => entity.Name == rq.Name;
}
