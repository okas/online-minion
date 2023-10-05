using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using OnlineMinion.Application.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseGetModelByIdReqHlr<TRequest, TResponse>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<TRequest, TResponse>
    where TRequest : IGetByIdRequest<TResponse>
{
    public async Task<ErrorOr<TResponse>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        var responseMessage = await apiClient.GetAsync(uri, ct).ConfigureAwait(false);

        return await HandleResponse(responseMessage).ConfigureAwait(false);
    }

    public virtual Uri BuildUri(TRequest rq) => new(
        $"{resource}/{rq.Id}",
        UriKind.RelativeOrAbsolute
    );

    private static async ValueTask<ErrorOr<TResponse>> HandleResponse(HttpResponseMessage response) =>
        response.StatusCode switch
        {
            HttpStatusCode.OK => await HandleExpectedHttpResult(response).ConfigureAwait(false),
            _ => Error.Failure(description: $"Server returned {response.StatusCode}"),
        };

    private static async ValueTask<ErrorOr<TResponse>> HandleExpectedHttpResult(HttpResponseMessage response)
    {
        if (await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false) is { } result)
        {
            return result;
        }

        return Error.Unexpected(description: $"Server returned {HttpStatusCode.OK} but response is empty");
    }
}
