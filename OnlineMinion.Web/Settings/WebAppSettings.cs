namespace OnlineMinion.Web.Settings;

public class WebAppSettings
{
    /// <summary>
    ///     Controls whether the response body should be buffered. See
    ///     <see
    ///         cref="Microsoft.AspNetCore.Components.WebAssembly.Http.WebAssemblyHttpRequestMessageExtensions.SetBrowserResponseStreamingEnabled">
    ///         WebAssemblyHttpRequestMessageExtensions.SetBrowserResponseStreamingEnabled
    ///     </see>
    /// </summary>
    public required bool BrowserResponseStreamingEnabled { get; set; }
}
