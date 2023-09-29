using ErrorOr;
using MediatR;

namespace OnlineMinion.Application.Contracts.Shared.Requests;

public interface IDeleteByIdCommand : IHasId, IRequest<ErrorOr<Deleted>>;
