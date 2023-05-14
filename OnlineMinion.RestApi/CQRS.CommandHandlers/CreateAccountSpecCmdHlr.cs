using MediatR;
using OnlineMinion.Contracts.Commands;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.CQRS.CommandHandlers;

public sealed class CreateAccountSpecCmdHlr : IRequestHandler<CreateAccountSpecCmd, AccountSpecResp>
{
    private readonly OnlineMinionDbContext _dbContext;

    public CreateAccountSpecCmdHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<AccountSpecResp> Handle(CreateAccountSpecCmd rq, CancellationToken ct)
    {
        var entry = await _dbContext.AccountSpecs.AddAsync(
            new(rq.Name, rq.Group, rq.Description),
            ct
        );

        await _dbContext.SaveChangesAsync(ct);

        return rq.ToResponse(entry.Entity.Id);
    }
}
