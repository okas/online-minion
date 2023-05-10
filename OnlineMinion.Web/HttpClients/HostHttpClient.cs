using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace OnlineMinion.Web.HttpClients;

public class HostHttpClient : BaseHttpClient
{
    public HostHttpClient(HttpClient httpClient, IWebAssemblyHostEnvironment hostEnv)
    {
        Client = httpClient;
        Client.BaseAddress = new(hostEnv.BaseAddress);
    }
}
