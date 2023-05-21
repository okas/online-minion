using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace OnlineMinion.Web.Infrastructure;

public class SetWebAssemblyStreamingOptionsHandler : DelegatingHandler
{
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken ct)
    {
        SetOption(request);

        return base.Send(request, ct);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        SetOption(request);

        return base.SendAsync(request, ct);
    }

    private static void SetOption(HttpRequestMessage request)
    {
        if (request.Method == HttpMethod.Get)
        {
            request.SetBrowserResponseStreamingEnabled(streamingEnabled: true);
        }
    }
}
