using Microsoft.Extensions.Options;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.HttpClients;

public class ApiHttpClient : BaseHttpClient
{
    public ApiHttpClient(HttpClient httpClient, IOptions<ApiClientSettings> options)
    {
        Client = httpClient;
        Client.BaseAddress = new(options.Value.Url);
    }
}
