using ErrorOr;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

[UsedImplicitly]
internal sealed class GetModelPagingInfoReqHlr<TRequest, TEntity>(IOnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<TRequest, PagingMetaInfo>
    where TRequest : IGetPagingInfoRequest
    where TEntity : class, IEntity<IId>
{
    public async Task<ErrorOr<PagingMetaInfo>> Handle(TRequest rq, CancellationToken ct)
    {
        int count;
        try
        {
            count = await dbContext.Set<TEntity>().CountAsync(ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            return Error.Failure(description: $"Error while getting paging info for {requestName}: {ex.Message}");
        }

        return new PagingMetaInfo(
            count,
            rq.PageSize,
            BasePagingParams.FirstPage
        );
    }
}
