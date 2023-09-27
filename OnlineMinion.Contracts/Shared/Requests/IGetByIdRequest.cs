using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IGetByIdRequest<TResponse> : IHasId, IRequest<ErrorOr<TResponse>>;
