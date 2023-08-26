using System.Net;
using ErrorOr;
using MediatR;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseCheckUniqueReqHlr<TRequest>(HttpClient apiClient)
    : IApiClientRequestHandler<TRequest, Success> where TRequest : IRequest<ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        var message = new HttpRequestMessage(HttpMethod.Head, uri);

        var result = await apiClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        return HandleResponse(result);
    }

    public abstract Uri BuildUri(TRequest rq);

    private static ErrorOr<Success> HandleResponse(HttpResponseMessage response) => response.StatusCode switch
    {
        HttpStatusCode.NoContent => Result.Success,
        HttpStatusCode.Conflict => Error.Conflict(),
        _ => Error.Unexpected(),
    };
}
