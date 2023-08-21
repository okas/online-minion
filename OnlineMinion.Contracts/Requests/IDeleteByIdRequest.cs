using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Requests;

public interface IDeleteByIdRequest : IHasIntId, IRequest<ErrorOr<Deleted>> { }
