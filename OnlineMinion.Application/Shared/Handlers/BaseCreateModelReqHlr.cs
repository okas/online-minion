using ErrorOr;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Application.Contracts.Shared.Responses;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

internal abstract class BaseCreateModelReqHlr<TRequest, TEntity>(IOnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<TRequest, ModelIdResp>
    where TRequest : ICreateCommand
    where TEntity : class, IEntity<IId>
{
    public async Task<ErrorOr<ModelIdResp>> Handle(TRequest rq, CancellationToken ct)
    {
        var entity = ToEntity(rq);

        await dbContext.Set<TEntity>().AddAsync(entity, ct).ConfigureAwait(false);

        return new ModelIdResp(entity.Id.Value);
    }

    protected abstract TEntity ToEntity(TRequest rq);
}
