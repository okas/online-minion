using ErrorOr;
using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.Requests;

public interface ICreateCommand : IRequest<ErrorOr<ModelIdResp>>, IUpsertCommand { }
