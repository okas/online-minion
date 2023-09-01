using System.Transactions;
using ErrorOr;
using MediatR;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.MediatorInfra.Behaviors;

/// <summary>
///     NB! Creates explicit transaction scope for each command request. If this behavior need altering, consider
///     changing type constraints or add other means of filtering.
///     <br />
///     Credit: https://youtu.be/sSIg3fpflI0
/// </summary>
/// <remarks>
///     Type constraints are supposed to defined pipeline "segment" only for command type requests. There is not other
///     filtering to prevent this behavior to be applied to other types of requests.
/// </remarks>
/// <typeparam name="TRequest">Request or model, constrained to <see cref="IUpsertCommand{TResponse}" />.</typeparam>
/// <typeparam name="TResponse">Response model, constrained to <see cref="IErrorOr" />.</typeparam>
public sealed class CommandUnitOfWorkBehavior<TRequest, TResponse>(OnlineMinionDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IUpsertCommand<TResponse>
    where TResponse : IErrorOr
{
    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest req, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var result = await next().ConfigureAwait(false);

        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);

        transactionScope.Complete();

        return result;
    }
}
