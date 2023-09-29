using ErrorOr;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

/// <inheritdoc cref="IRequestResponseStreaming" />
internal interface ICollectionRequestResponseStreaming : IRequestResponseStreaming
{
    /// <summary>
    ///     Orchestrates the API request, response transformation and error handling.
    /// </summary>
    /// <param name="apiClient"></param>
    /// <param name="uri"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns>
    ///     If HTTP request returns 2xx result then result is read as model stream;<br />
    ///     otherwise <see cref="Error" /> with with status code and reason phrase.
    /// </returns>
    protected static async ValueTask<ErrorOr<IAsyncEnumerable<TResponse>>> GetApiResponse<TResponse>(
        HttpClient        apiClient,
        Uri               uri,
        CancellationToken ct
    )
    {
        var responseMessage = await GetHttpRequestResponse(apiClient, uri, ct).ConfigureAwait(false);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return ToStreamResponseApiError(responseMessage);
        }

        var modelsAsyncStream = await GetResultAsStreamAsync<TResponse>(responseMessage, ct).ConfigureAwait(false);

        return ErrorOrFactory.From(modelsAsyncStream);
    }
}
