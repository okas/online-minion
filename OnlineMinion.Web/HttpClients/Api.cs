using Microsoft.Extensions.Options;
using OnlineMinion.Web.Settings;

namespace OnlineMinion.Web.HttpClients;

public class Api
{
    public Api(HttpClient httpClient, IOptions<ApiSettings> options)
    {
        Client = httpClient;
        Client.BaseAddress = new(options.Value.Url);
    }

    public HttpClient Client { get; }
}
