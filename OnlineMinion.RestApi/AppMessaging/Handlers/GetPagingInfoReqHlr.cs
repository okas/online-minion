using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class GetPagingInfoReqHlr<TEntity> : IRequestHandler<GetPagingMetaInfoReq<TEntity>, PagingMetaInfo>
    where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;

    public GetPagingInfoReqHlr(OnlineMinionDbContext dbContext) => _dbSet = dbContext.Set<TEntity>();

    public async Task<PagingMetaInfo> Handle(GetPagingMetaInfoReq<TEntity> rq, CancellationToken ct) =>
        new(await _dbSet.CountAsync(ct), rq.PageSize);
}
