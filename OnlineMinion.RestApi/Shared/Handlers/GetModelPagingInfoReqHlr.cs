using ErrorOr;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineMinion.Common;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.Shared.Handlers;

[UsedImplicitly]
internal sealed class GetModelPagingInfoReqHlr<TRequest, TEntity>(
        OnlineMinionDbContext                                dbContext,
        ILogger<GetModelPagingInfoReqHlr<TRequest, TEntity>> logger
    )
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
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogCritical(
                ex,
                "Error while getting paging info for {RequestType}: {ErrorMessage}",
                requestName,
                ex.Message
            );

            return Error.Failure(
                $"Error while getting paging info for {requestName}: {ex.Message}",
                ex.InnerException?.Message ?? string.Empty
            );
        }

        return new PagingMetaInfo(
            count,
            rq.PageSize,
            BasePagingParams.FirstPage
        );
    }
}
