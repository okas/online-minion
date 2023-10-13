namespace OnlineMinion.RestApi.Client.Settings;

public sealed class ApiClientSettings
{
    public const string ConfigurationSection = "ApiClient";

    public required string Url { get; set; }
}
