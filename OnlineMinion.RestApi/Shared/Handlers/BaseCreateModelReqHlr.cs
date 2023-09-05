using ErrorOr;
using OnlineMinion.Common;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.Shared.Handlers;

internal abstract class BaseCreateModelReqHlr<TRequest, TEntity>(OnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<TRequest, ModelIdResp>
    where TRequest : ICreateCommand
    where TEntity : BaseEntity
{
    public async Task<ErrorOr<ModelIdResp>> Handle(TRequest rq, CancellationToken ct)
    {
        var entry = await dbContext.Set<TEntity>()
            .AddAsync(ToEntity(rq), ct)
            .ConfigureAwait(false);

        return new ModelIdResp(entry.Entity.Id);
    }

    protected abstract TEntity ToEntity(TRequest rq);
}