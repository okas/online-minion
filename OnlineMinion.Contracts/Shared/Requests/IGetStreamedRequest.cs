using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IGetStreamedRequest<TResponse> : IRequest<ErrorOr<IAsyncEnumerable<TResponse>>>;
