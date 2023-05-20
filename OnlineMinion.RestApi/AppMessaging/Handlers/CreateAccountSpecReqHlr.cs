using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class CreateAccountSpecReqHlr : IRequestHandler<CreateAccountSpecReq, ModelIdResp>
{
    private readonly OnlineMinionDbContext _dbContext;

    public CreateAccountSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<ModelIdResp> Handle(CreateAccountSpecReq rq, CancellationToken ct)
    {
        var entry = await _dbContext.AccountSpecs.AddAsync(
                new(rq.Name, rq.Group, rq.Description),
                ct
            )
            .ConfigureAwait(false);

        await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);

        return new(entry.Entity.Id);
    }
}
