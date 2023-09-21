namespace OnlineMinion.RestApi.Client.HttpRequestMessageHandlers;

/// <summary>
///     Takes in action to be executed on the request pipeline.
/// </summary>
/// <remarks>
///     It allows to execute any action on the request pipeline. It is useful for avoiding adding unnecessary
///     dependencies to API Client assembly.<br />
///     This handler is registered in API Client assembly but as `services.<b>Try</b>AddTransient` and without
///     action (no-op). This means that if the  consuming application registers a handler with the same type,
///     then the consuming application's handler will be used.
/// </remarks>
/// <param name="pipelineAction">
///     Action is optional (no-op), because handler is registered by API Client, but only it's
///     consumers actions make sense in pipeline.
/// </param>
public class DelegatedRequestHandler(Action<HttpRequestMessage>? pipelineAction = default) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        pipelineAction?.Invoke(request);

        return base.SendAsync(request, ct);
    }
}
