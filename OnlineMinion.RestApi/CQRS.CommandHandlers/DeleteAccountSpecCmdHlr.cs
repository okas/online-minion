using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.Commands;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.CQRS.CommandHandlers;

public sealed class DeleteAccountSpecCmdHlr : IRequestHandler<DeleteAccountSpecCmd, bool>
{
    private readonly OnlineMinionDbContext _dbContext;

    public DeleteAccountSpecCmdHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public Task<bool> Handle(DeleteAccountSpecCmd rq, CancellationToken ct)
    {
        var task = _dbContext.AccountSpecs
            .Where(a => a.Id == rq.Id)
            .ExecuteDeleteAsync(ct);

        return Task.FromResult(task.Result > 0);
    }
}
