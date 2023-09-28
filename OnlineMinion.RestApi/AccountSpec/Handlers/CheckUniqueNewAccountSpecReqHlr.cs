using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewAccountSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCheckUniqueModelReqHlr<CheckAccountSpecUniqueNewReq, Domain.AccountSpec>(dbContext)
{
    protected override Expression<Func<Domain.AccountSpec, bool>> GetConflictPredicate(
        CheckAccountSpecUniqueNewReq rq
    ) =>
        entity => entity.Name == rq.MemberValue;
}
