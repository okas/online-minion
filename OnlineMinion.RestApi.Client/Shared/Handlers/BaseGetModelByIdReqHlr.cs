using System.Globalization;
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

        return await HandleResponse(responseMessage);
    }

    public virtual Uri BuildUri(TRequest rq) => new(
        string.Create(CultureInfo.InvariantCulture, $"{resource}/{rq.Id}"),
        UriKind.RelativeOrAbsolute
    );

    private static async ValueTask<ErrorOr<TResponse>> HandleResponse(HttpResponseMessage response) =>
        response.StatusCode switch
        {
            HttpStatusCode.OK =>
                await response.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false) is { } result
                    ? result
                    : Error.Unexpected($"Server returned {HttpStatusCode.OK} but response is empty"),
            _ => Error.Failure($"Server returned {response.StatusCode}"),
        };
}
