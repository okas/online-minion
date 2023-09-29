using MediatR;

namespace OnlineMinion.Application.RequestValidation;

/// <summary>
///     Decorator interface (initial intention). For FluentValidator async API based validation, that uses MediatR for
///     request sending.
/// </summary>
/// <remarks>It exposes only some oft MediatR's own API, just enough to send out requests.</remarks>
public interface IAsyncValidatorSender
{
    /// <summary>
    ///     Asynchronously send a request to a single handler
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="rq">Request object</param>
    /// <param name="ct">Optional cancellation token</param>
    /// <returns>A task that represents the send operation. The task result contains the handler response</returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> rq, CancellationToken ct = default);
}
