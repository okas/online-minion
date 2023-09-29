using MediatR;

namespace OnlineMinion.Application.RequestValidation;

/// <summary>For FluentValidator async API based validation, that uses MediatR for request sending.</summary>
/// <remarks>It exposes only some oft MediatR's own API, just enough to send out requests.</remarks>
public interface IAsyncValidatorSender
{
    /// <summary>
    ///     Asynchronously send a request to a single handler
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <param name="request">Request object</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>A task that represents the send operation. The task result contains the handler response</returns>
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously send an object request to a single handler via dynamic dispatch
    /// </summary>
    /// <param name="request">Request object</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>A task that represents the send operation. The task result contains the type erased handler response</returns>
    Task<object?> Send(object request, CancellationToken cancellationToken = default);
}
