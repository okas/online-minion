using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IDeleteByIdCommand : IHasIntId, IRequest<ErrorOr<Deleted>>;
