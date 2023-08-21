using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.AppMessaging;

public interface IDeleteByIdRequest : IHasIntId, IRequest<ErrorOr<Deleted>> { }
