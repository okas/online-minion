using ErrorOr;
using MediatR;
using OnlineMinion.Common;

namespace OnlineMinion.RestApi.Client.Shared;

internal interface IApiClientRequestHandler<in TRequest, TResult> : IApiRequestHandler<TRequest, TResult>
    where TRequest : IRequest<ErrorOr<TResult>>
{
    Uri BuildUri(TRequest rq);
}
