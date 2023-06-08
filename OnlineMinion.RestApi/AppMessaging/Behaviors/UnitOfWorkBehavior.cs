using System.Transactions;
using MediatR;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AppMessaging.Behaviors;

/// <summary>
///     Credit: https://youtu.be/sSIg3fpflI0
/// </summary>
public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : notnull
{
    private readonly OnlineMinionDbContext _dbContext;
    public UnitOfWorkBehavior(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest req, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var result = await next().ConfigureAwait(false);

        await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);

        transactionScope.Complete();

        return result;
    }
}
