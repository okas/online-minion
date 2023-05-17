using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class CreateAccountSpecReqHlr : IRequestHandler<CreateAccountSpecReq, AccountSpecResp>
{
    private readonly OnlineMinionDbContext _dbContext;

    public CreateAccountSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<AccountSpecResp> Handle(CreateAccountSpecReq rq, CancellationToken ct)
    {
        var entry = await _dbContext.AccountSpecs.AddAsync(
            new(rq.Name, rq.Group, rq.Description),
            ct
        );

        await _dbContext.SaveChangesAsync(ct);

        return rq.ToResponse(entry.Entity.Id);
    }
}