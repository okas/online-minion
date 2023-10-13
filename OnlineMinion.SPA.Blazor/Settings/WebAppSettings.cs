namespace OnlineMinion.SPA.Blazor.Settings;

public class WebAppSettings
{
    public const string ConfigurationSection = "WebApp";

    /// <summary>
    ///     Controls whether the response body should be buffered. See
    ///     <see
    ///         cref="Microsoft.AspNetCore.Components.WebAssembly.Http.WebAssemblyHttpRequestMessageExtensions.SetBrowserResponseStreamingEnabled">
    ///         WebAssemblyHttpRequestMessageExtensions.SetBrowserResponseStreamingEnabled
    ///     </see>
    /// </summary>
    public required bool BrowserResponseStreamingEnabled { get; set; }
}
