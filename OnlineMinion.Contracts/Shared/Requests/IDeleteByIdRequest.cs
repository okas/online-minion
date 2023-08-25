using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IDeleteByIdRequest : IHasIntId, IRequest<ErrorOr<Deleted>> { }
