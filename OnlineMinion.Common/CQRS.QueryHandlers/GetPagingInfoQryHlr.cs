using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Queries;
using OnlineMinion.Data;

namespace OnlineMinion.Common.CQRS.QueryHandlers;

public sealed class GetPagingInfoQryHlr<TEntity> : IRequestHandler<GetPagingMetaInfoQry<TEntity>, PagingMetaInfo>
    where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;

    public GetPagingInfoQryHlr(OnlineMinionDbContext dbContext) => _dbSet = dbContext.Set<TEntity>();

    public async Task<PagingMetaInfo> Handle(GetPagingMetaInfoQry<TEntity> rq, CancellationToken ct) =>
        new(await _dbSet.CountAsync(ct), rq.PageSize);
}
