using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.Commands;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.CQRS.CommandHandlers;

public sealed class UpdateAccountSpecCmdHlr : IRequestHandler<UpdateAccountSpecCmd, bool>
{
    private readonly OnlineMinionDbContext _dbContext;

    public UpdateAccountSpecCmdHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public Task<bool> Handle(UpdateAccountSpecCmd rq, CancellationToken ct)
    {
        var task = _dbContext.AccountSpecs
            .Where(a => a.Id == rq.Id)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(a => a.Name, rq.Name)
                    .SetProperty(a => a.Group, rq.Group)
                    .SetProperty(a => a.Description, rq.Description),
                ct
            );

        return Task.FromResult(task.Result > 0);
    }
}
