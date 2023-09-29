using ErrorOr;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

[UsedImplicitly]
internal sealed class GetModelPagingInfoReqHlr<TRequest, TEntity>(
        IOnlineMinionDbContext                               dbContext,
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
