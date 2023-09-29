using System.Linq.Expressions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Common;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

internal abstract class GetSomeModelDescriptorsReqHlr<TEntity, TResponse>(IOnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<GetSomeModelDescriptorsReq<TResponse>, IAsyncEnumerable<TResponse>>
    where TEntity : BaseEntity
    where TResponse : IHasId
{
    protected abstract Expression<Func<TEntity, TResponse>> Projection { get; }

    public Task<ErrorOr<IAsyncEnumerable<TResponse>>> Handle(
        GetSomeModelDescriptorsReq<TResponse> rq,
        CancellationToken                     ct
    )
    {
        var query = dbContext.Set<TEntity>().AsNoTracking();

        var resultStream = query.Select(Projection).AsAsyncEnumerable();

        return Task.FromResult(ErrorOrFactory.From(resultStream));
    }
}
