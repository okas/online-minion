using ErrorOr;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.Shared.Handlers;

[UsedImplicitly]
internal sealed class GetModelPagingInfoReqHlr<TRequest, TEntity>(OnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<TRequest, PagingMetaInfo>
    where TRequest : IGetPagingInfoRequest
    where TEntity : BaseEntity
{
    public async Task<ErrorOr<PagingMetaInfo>> Handle(TRequest rq, CancellationToken ct)
    {
        int count;
        try
        {
            count = await dbContext.Set<TEntity>().CountAsync(ct).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            return Error.Failure(
                e.Message,
                e.InnerException?.Message ?? string.Empty
            );
        }

        return new PagingMetaInfo(
            count,
            rq.PageSize,
            BasePagingParams.FirstPage
        );
    }
}
