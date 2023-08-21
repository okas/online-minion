using ErrorOr;
using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.AppMessaging;

public interface ICreateCommand : IRequest<ErrorOr<ModelIdResp>>, IUpsertCommand { }
