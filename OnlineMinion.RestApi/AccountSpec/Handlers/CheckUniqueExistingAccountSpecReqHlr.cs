using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingAccountSpecReqHlr(OnlineMinionDbContext dbContext)
    : BaseCheckUniqueModelReqHlr<CheckAccountSpecUniqueExistingReq, Data.Entities.AccountSpec>(dbContext)
{
    protected override Expression<Func<Data.Entities.AccountSpec, bool>> GetConflictPredicate(
        CheckAccountSpecUniqueExistingReq rq
    ) =>
        entity => entity.Name == rq.Name && entity.Id != rq.ExceptId;
}
