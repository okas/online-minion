using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public record GetSomeModelDescriptorsReq<TResponse> : IRequest<ErrorOr<IAsyncEnumerable<TResponse>>>;
