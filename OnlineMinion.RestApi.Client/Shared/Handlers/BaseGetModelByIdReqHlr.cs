using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseGetModelByIdReqHlr<TRequest, TResponse>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<TRequest, TResponse>
    where TRequest : IGetByIdRequest<TResponse>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly string MsgEmptyContent = $"Server returned {(int)HttpStatusCode.OK} but response is empty.";

    // ReSharper disable once StaticMemberInGenericType
    private static readonly string MsgUnknownContent = $"Server returned content with response '{(int)HttpStatusCode.OK
    }', but header `Content-Type` is unknown, unable to serialize.";

    public async Task<ErrorOr<TResponse>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        var responseMessage = await apiClient.GetAsync(uri, ct).ConfigureAwait(false);

        return await HandleResponse(responseMessage, ct).ConfigureAwait(false);
    }

    public virtual Uri BuildUri(TRequest rq) => new(
        $"{resource}/{rq.Id}",
        UriKind.RelativeOrAbsolute
    );

    private async ValueTask<ErrorOr<TResponse>> HandleResponse(
        HttpResponseMessage response,
        CancellationToken   ct
    ) => response switch
    {
        { StatusCode: HttpStatusCode.OK, Content.Headers.ContentLength: > 0, } =>
            await HandleExpectedHttpResult(response, ct).ConfigureAwait(false),

        { StatusCode: HttpStatusCode.OK, Content.Headers.ContentLength: 0, } =>
            Error.Unexpected(description: MsgEmptyContent),

        _ => Error.Failure(description: $"Server returned {response.StatusCode}"),
    };

    private async ValueTask<ErrorOr<TResponse>> HandleExpectedHttpResult(
        HttpResponseMessage response,
        CancellationToken   ct
    )
    {
        if (await ToResponse(response, ct).ConfigureAwait(false) is { } result)
        {
            return result;
        }

        return Error.Failure(description: MsgUnknownContent);
    }

    protected virtual ValueTask<TResponse?> ToResponse(HttpResponseMessage response, CancellationToken ct) =>
        new(response.Content.ReadFromJsonAsync<TResponse>(ct));
}
