using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingAccountSpecReqHlr(OnlineMinionDbContext dbContext)
    : BaseCheckUniqueModelReqHlr<CheckAccountSpecUniqueExistingReq, Domain.AccountSpec>(dbContext)
{
    protected override Expression<Func<Domain.AccountSpec, bool>> GetConflictPredicate(
        CheckAccountSpecUniqueExistingReq rq
    ) =>
        entity => entity.Name == rq.MemberValue && entity.Id != rq.OwnId;
}
