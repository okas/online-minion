using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.Commands;
using OnlineMinion.Data;

namespace OnlineMinion.Common.CQRS.CommandHandlers;

public sealed class DeleteAccountSpecCmdHlr : IRequestHandler<DeleteAccountSpecCmd, int>
{
    private readonly OnlineMinionDbContext _dbContext;

    public DeleteAccountSpecCmdHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public Task<int> Handle(DeleteAccountSpecCmd rq, CancellationToken ct) =>
        _dbContext.AccountSpecs
            .Where(a => a.Id == rq.Id)
            .ExecuteDeleteAsync(ct);
}
