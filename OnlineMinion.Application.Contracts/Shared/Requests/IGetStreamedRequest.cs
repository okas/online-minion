using ErrorOr;
using MediatR;

namespace OnlineMinion.Application.Contracts.Shared.Requests;

public interface IGetStreamedRequest<TResponse> : IRequest<ErrorOr<IAsyncEnumerable<TResponse>>>;
