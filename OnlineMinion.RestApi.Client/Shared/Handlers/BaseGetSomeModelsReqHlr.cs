using ErrorOr;
using MediatR;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseGetSomeModelsReqHlr<TRequest, TResponse>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<TRequest, IAsyncEnumerable<TResponse>>,
        IRequestResponseStreaming<TResponse>
    where TRequest : IRequest<ErrorOr<IAsyncEnumerable<TResponse>>>
{
    public async Task<ErrorOr<IAsyncEnumerable<TResponse>>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        var message = new HttpRequestMessage(HttpMethod.Get, uri);

        var responseMessage = await IRequestResponseStreaming<TResponse>
            .GetRequestResponse(apiClient, message, ct)
            .ConfigureAwait(false);

        return await HandleResponse(responseMessage, ct).ConfigureAwait(false);
    }

    public virtual Uri BuildUri(TRequest _) => resource;

    private static async ValueTask<ErrorOr<IAsyncEnumerable<TResponse>>> HandleResponse(
        HttpResponseMessage responseMessage,
        CancellationToken   ct
    )
    {
        if (!responseMessage.IsSuccessStatusCode)
        {
            return Error.Unexpected();
        }

        var modelsAsyncStream = await IRequestResponseStreaming<TResponse>
            .GetResultAsStreamAsync(responseMessage, ct)
            .ConfigureAwait(false);

        return ErrorOrFactory.From(modelsAsyncStream);
    }
}
