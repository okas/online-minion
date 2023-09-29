using ErrorOr;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Application.Contracts.Shared.Responses;
using OnlineMinion.Common;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

internal abstract class BaseCreateModelReqHlr<TRequest, TEntity>(IOnlineMinionDbContext dbContext)
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
