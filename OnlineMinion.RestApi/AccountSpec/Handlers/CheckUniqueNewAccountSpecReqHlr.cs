using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewAccountSpecReqHlr(OnlineMinionDbContext dbContext)
    : BaseCheckUniqueModelReqHlr<CheckAccountSpecUniqueNewReq, Data.Entities.AccountSpec>(dbContext)
{
    protected override Expression<Func<Data.Entities.AccountSpec, bool>> GetConflictPredicate(
        CheckAccountSpecUniqueNewReq rq
    ) =>
        entity => entity.Name == rq.Name;
}
