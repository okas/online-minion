using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.AppMessaging;

public interface IUpdateCommand : IHasIntId, IRequest<ErrorOr<Updated>>, IUpsertCommand { }
