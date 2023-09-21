namespace OnlineMinion.RestApi.Client.Settings;

public sealed class ApiProviderSettings
{
    public required string Url { get; set; }

    /// <summary>
    ///     Default API version to set at message handling middleware, if not yet specified
    ///     <see cref="HttpRequestMessage.Headers">HttpRequestMessage.Headers</see>.
    /// </summary>
    public required string DefaultApiVersion { get; set; }
}
