namespace OnlineMinion.Web.HttpClients;

public abstract class BaseHttpClient
{
    public HttpClient Client { get; protected init; } = null!;
}
