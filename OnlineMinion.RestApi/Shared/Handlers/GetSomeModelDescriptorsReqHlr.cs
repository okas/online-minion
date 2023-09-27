using System.Linq.Expressions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Data;
using OnlineMinion.Domain;

namespace OnlineMinion.RestApi.Shared.Handlers;

internal abstract class GetSomeModelDescriptorsReqHlr<TEntity, TResponse>(OnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<GetSomeModelDescriptorsReq<TResponse>, IAsyncEnumerable<TResponse>>
    where TEntity : BaseEntity
    where TResponse : IHasIntId
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
