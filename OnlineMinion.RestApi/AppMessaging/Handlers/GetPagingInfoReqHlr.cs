using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Data;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.AppMessaging.Requests;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

internal sealed class GetPagingInfoReqHlr<TEntity> : IRequestHandler<GetPagingMetaInfoReq<TEntity>, PagingMetaInfo>
    where TEntity : BaseEntity
{
    private readonly OnlineMinionDbContext _dbContext;
    public GetPagingInfoReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<PagingMetaInfo> Handle(GetPagingMetaInfoReq<TEntity> rq, CancellationToken ct) =>
        new(
            await _dbContext.Set<TEntity>().CountAsync(ct).ConfigureAwait(false),
            rq.PageSize,
            BasePagingParams.FirstPage
        );
}
