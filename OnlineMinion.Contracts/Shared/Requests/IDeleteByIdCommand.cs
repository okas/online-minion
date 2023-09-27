using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IDeleteByIdCommand : IHasId, IRequest<ErrorOr<Deleted>>;
