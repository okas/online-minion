using System.Text.Json;
using ErrorOr;

namespace OnlineMinion.RestApi.Client.Shared;

/// <summary>
///     Manages Wasm Browser Streaming Behavior. Has default static implementations for to handel sending request  with
///     completion option set to
///     <see cref="HttpCompletionOption.ResponseHeadersRead">HttpCompletionOption.ResponseHeadersRead</see> and reading
///     response as stream.<br />
///     Important: <b>mentioned behavior</b> and <b>http completion</b> are requirements to get streaming behavior working!
///     See remarks.
/// </summary>
/// <remarks>
///     Current assembly do not hold references to WebAssembly specific dependencies! To get behavior working, the consumer
///     has to configure it as DelegatingHandler for HttpClientFactory. This delegate must call
///     <c>httpRequestMessage.SetBrowserResponseStreamingEnabled</c> in its overriden <c>SendAsync(..)</c>  method.<br />
///     See:
///     <a
///         href="https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.webassembly.http.webassemblyhttprequestmessageextensions.setbrowserresponsestreamingenabled?">
///         WebAssemblyHttpRequestMessageExtensions.SetBrowserResponseStreamingEnabled
///     </a>
///     <br />
///     See <a href="https://developer.mozilla.org/en-US/docs/Web/API/ReadableStream">MDN: ReadableStream</a>
/// </remarks>
internal interface IRequestResponseStreaming
{
    protected static readonly JsonSerializerOptions DefaultDeserializationOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultBufferSize = 16,
    };

    /// <summary>
    ///     Has default implementation, where the call to
    ///     <see cref="HttpClient.SendAsync(System.Net.Http.HttpRequestMessage, HttpCompletionOption)" /> method is made
    ///     with completion option set to <see cref="HttpCompletionOption.ResponseHeadersRead" />.
    /// </summary>
    /// <remarks>
    ///     As <c>HttpClientFactory</c> provided middleware for <b>request message</b> handling do not support
    ///     manipulating the completion option, this method is used to get the response with
    ///     <see cref="HttpCompletionOption.ResponseHeadersRead" />.
    /// </remarks>
    /// <param name="client"></param>
    /// <param name="uri"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    protected static Task<HttpResponseMessage> GetHttpRequestResponse(
        HttpClient        client,
        Uri               uri,
        CancellationToken ct
    )
    {
        var message = new HttpRequestMessage(HttpMethod.Get, uri);

        return client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct);
    }

    /// <summary>
    ///     Using JSON deserialization, reads the response as stream of models of <typeparamref name="TResponse" />.
    /// </summary>
    /// <param name="httpResponse"></param>
    /// <param name="ct"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <remarks>Uses deserialization options from <see cref="DefaultDeserializationOptions" />.</remarks>
    /// <returns></returns>
    protected static async ValueTask<IAsyncEnumerable<TResponse>> GetResultAsStreamAsync<TResponse>(
        HttpResponseMessage httpResponse,
        CancellationToken   ct
    )
    {
        var stream = await httpResponse.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);

        return JsonSerializer.DeserializeAsyncEnumerable<TResponse>(stream, DefaultDeserializationOptions, ct)!;
    }

    protected static Error ToStreamResponseApiError(HttpResponseMessage httpResponse) =>
        Error.Unexpected(
            httpResponse.StatusCode.ToString(),
            $"Unexpected response from server: {httpResponse.ReasonPhrase}"
        );
}
