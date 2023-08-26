using System.Linq.Expressions;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.Shared.Handlers;

internal abstract class BaseCheckUniqueModelReqHlr<TRequest, TEntity> : IRequestHandler<TRequest, ErrorOr<Success>>
    where TRequest : IRequest<ErrorOr<Success>>
    where TEntity : BaseEntity
{
    private readonly OnlineMinionDbContext _dbContext;
    protected BaseCheckUniqueModelReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<ErrorOr<Success>> Handle(TRequest rq, CancellationToken ct)
    {
        var predicate = GetConflictPredicate(rq);

        var result = await _dbContext.Set<TEntity>()
            .AnyAsync(predicate, ct)
            .ConfigureAwait(false);

        return result ? Error.Conflict() : Result.Success;
    }

    protected abstract Expression<Func<TEntity, bool>> GetConflictPredicate(TRequest rq);
}
