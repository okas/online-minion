using System.Text.Json;

namespace OnlineMinion.RestApi.Client.Shared;

internal interface IRequestResponseStreaming<TResponse>
{
    protected static Task<HttpResponseMessage> GetRequestResponse(
        HttpClient         client,
        HttpRequestMessage message,
        CancellationToken  ct
    ) =>
        client.SendAsync(
            /* Important: *browser streaming* and *http completion* are requirements to get streaming behavior working.
             * NB! It is expected to be configured as DelegatingHandler for HttpClientFactory!
             * This way current assembly do not hold references to WebAssembly specific dependencies. */
            // message.SetBrowserResponseStreamingEnabled(true),
            message,
            HttpCompletionOption.ResponseHeadersRead,
            ct
        );

    protected static async ValueTask<IAsyncEnumerable<TResponse>> GetResultAsStreamAsync(
        HttpResponseMessage httpResponseMessage,
        CancellationToken   ct
    )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultBufferSize = 16,
        };

        var stream = await httpResponseMessage.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);

        return JsonSerializer.DeserializeAsyncEnumerable<TResponse>(stream, options, ct)!;
    }
}
