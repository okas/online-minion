using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Common.Requests;

public interface IDeleteByIdRequest : IHasIntId, IRequest<ErrorOr<Deleted>> { }
