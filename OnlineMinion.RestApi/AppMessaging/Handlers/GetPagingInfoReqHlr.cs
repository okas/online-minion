using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Data;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.AppMessaging.Requests;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class GetPagingInfoReqHlr<TEntity> : IRequestHandler<GetPagingMetaInfoReq<TEntity>, PagingMetaInfo>
    where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;

    public GetPagingInfoReqHlr(OnlineMinionDbContext dbContext) => _dbSet = dbContext.Set<TEntity>();

    public async Task<PagingMetaInfo> Handle(GetPagingMetaInfoReq<TEntity> rq, CancellationToken ct) =>
        new(await _dbSet.CountAsync(ct).ConfigureAwait(false), rq.PageSize);
}
