using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Options;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.Configuration;

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
