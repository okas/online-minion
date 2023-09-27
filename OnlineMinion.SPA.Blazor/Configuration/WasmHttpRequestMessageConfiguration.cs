using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Options;
using OnlineMinion.SPA.Blazor.Settings;

namespace OnlineMinion.SPA.Blazor.Configuration;

/// <summary>
///     This class is used to configure the <see cref="HttpRequestMessage" /> before it is sent to the server.
///     <see cref="WasmHttpRequestMessageConfiguration.EnableBrowserResponseStreamingForGet" /> is used to enable reading
///     response as a stream for GET requests.
/// </summary>
/// <remarks>
///     This class is defined in WebAssembly project, so it can be used to configure the request before it is sent to
///     the server. This way any API client assembly do not have to hold references to browser specific dependencies.
///     It is supposed to be injected into DelegateHandler, which in turn must be registered in HttpClientFactory.<br />
///     Also, it is important to set
///     <see cref="WebAppSettings.BrowserResponseStreamingEnabled">WebAppSettings.BrowserResponseStreamingEnabled</see>
///     to <c>true</c> in <c>appsettings.json</c>.<br />
///     For the reading response as a stream to work, the request must be sent with
///     <see cref="HttpClient.SendAsync(System.Net.Http.HttpRequestMessage, HttpCompletionOption)" /> method with the
///     completion option set to <see cref="HttpCompletionOption.ResponseHeadersRead" />.<br />
///     This call is made in
///     <see cref="OnlineMinion.RestApi.Client.Shared.IRequestResponseStreaming.GetRequestResponse">
///         IRequestResponseStreaming.GetRequestResponse
///     </see>
///     , in its default implementation.
/// </remarks>
/// <param name="options">
///     Access to options to control state of Wasm Browser Response Streaming behavior.
/// </param>
public class WasmHttpRequestMessageConfiguration(IOptions<WebAppSettings> options)
{
    public void EnableBrowserResponseStreamingForGet(HttpRequestMessage httpRequestMessage)
    {
        if (httpRequestMessage.Method == HttpMethod.Get)
        {
            httpRequestMessage.SetBrowserResponseStreamingEnabled(options.Value.BrowserResponseStreamingEnabled);
        }
    }
}
