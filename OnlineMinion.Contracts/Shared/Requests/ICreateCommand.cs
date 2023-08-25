using ErrorOr;
using MediatR;
using OnlineMinion.Contracts.Shared.Responses;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface ICreateCommand : IRequest<ErrorOr<ModelIdResp>>, IUpsertCommand { }
