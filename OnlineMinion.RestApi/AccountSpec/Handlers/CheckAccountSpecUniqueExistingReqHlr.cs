using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Common.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckAccountSpecUniqueExistingReqHlr
    : BaseUniquenessCheckReqHlr<CheckAccountSpecUniqueExistingReq, Data.Entities.AccountSpec>
{
    public CheckAccountSpecUniqueExistingReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    protected override Expression<Func<Data.Entities.AccountSpec, bool>> GetConflictPredicate(
        CheckAccountSpecUniqueExistingReq rq
    ) =>
        entity => entity.Name == rq.Name && entity.Id != rq.ExceptId;
}
