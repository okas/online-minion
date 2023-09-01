using System.Globalization;
using System.Text.Json;
using ErrorOr;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class GetSomeModelDescriptorsReqHlr<TResponse>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<GetSomeModelDescriptorsReq<TResponse>, IAsyncEnumerable<TResponse>>
{
    public async Task<ErrorOr<IAsyncEnumerable<TResponse>>> Handle(
        GetSomeModelDescriptorsReq<TResponse> rq,
        CancellationToken                     ct
    )
    {
        var uri = BuildUri(rq);

        var message = new HttpRequestMessage(HttpMethod.Get, uri);

        var responseMessage = await GetRequestResponse(message, ct).ConfigureAwait(false);

        var modelsAsyncStream = await GetResultAsStreamAsync(responseMessage, ct).ConfigureAwait(false);

        return ErrorOrFactory.From(modelsAsyncStream);
    }

    public virtual Uri BuildUri(GetSomeModelDescriptorsReq<TResponse> rq) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{resource}/descriptors"
        ),
        UriKind.RelativeOrAbsolute
    );

    private Task<HttpResponseMessage> GetRequestResponse(HttpRequestMessage message, CancellationToken ct) =>
        apiClient.SendAsync(
            /* Important: *browser streaming* and *http completion* are requirements to get streaming behavior working.
             * NB! It is expected to be configured as DelegatingHandler for HttpClientFactory!
             * This way current assembly do not hold references to WebAssembly specific dependencies. */
            // message.SetBrowserResponseStreamingEnabled(true),
            message,
            HttpCompletionOption.ResponseHeadersRead,
            ct
        );

    private static async ValueTask<IAsyncEnumerable<TResponse>> GetResultAsStreamAsync(
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

        // TODO: Compiler error about nullabiliti mismatch means ErrorOR should be implemented.
        return JsonSerializer.DeserializeAsyncEnumerable<TResponse>(stream, options, ct);
    }
}
