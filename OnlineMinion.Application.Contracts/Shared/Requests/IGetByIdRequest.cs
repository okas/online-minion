using ErrorOr;
using MediatR;

namespace OnlineMinion.Application.Contracts.Shared.Requests;

public interface IGetByIdRequest<TResponse> : IHasId, IRequest<ErrorOr<TResponse>>;
