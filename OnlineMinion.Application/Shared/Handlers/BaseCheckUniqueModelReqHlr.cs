using System.Linq.Expressions;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

/// <summary>
///     Generic handler for checking the uniqueness of a new entity by it's member value(s). The member value is passed in
///     the request instance. The implementation of this class provides both request and entity type information. Also the
///     predicate is provided
///     by the implementation, that is used to check the uniqueness of the entity.
/// </summary>
/// <remarks>
///     Validation requests are meant to be quick, therefore the could be used in parallel.
///     But... as MediatR as of 13.10.2023 do not support Microsoft DI Keyed Services registration at all, then one way
///     to get the transient DbContext per validation request is to use the scope factory. The scope factory is injected
///     into the handler by the DI container and is used to resolve the DbContext instance.
///     <a href="https://github.com/jbogard/MediatR/issues/942">MediatR issue 942</a>
/// </remarks>
/// <param name="scopeFactory">
/// </param>
/// <typeparam name="TRequest">
///     Request type provides the input by means of the member value(s) to check the uniqueness.
/// </typeparam>
/// <typeparam name="TEntity">
///     Entity type states which domain model type is used to check the uniqueness. Or in other words, which table is
///     queried.
/// </typeparam>
internal abstract class BaseCheckUniqueModelReqHlr<TRequest, TEntity>(IServiceScopeFactory scopeFactory)
    : IRequestHandler<TRequest, ErrorOr<Success>>
    where TRequest : IRequest<ErrorOr<Success>>
    where TEntity : class, IEntity<IId>
{
    public async Task<ErrorOr<Success>> Handle(TRequest rq, CancellationToken ct)
    {
        var predicate = GetConflictPredicate(rq);

        bool isConflict;

        using (var scope = scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IOnlineMinionDbContext>();

            isConflict = await dbContext.Set<TEntity>()
                .AnyAsync(predicate, ct)
                .ConfigureAwait(false);
        }

        return isConflict
            ? Error.Conflict()
            : Result.Success;
    }

    protected abstract Expression<Func<TEntity, bool>> GetConflictPredicate(TRequest rq);
}
