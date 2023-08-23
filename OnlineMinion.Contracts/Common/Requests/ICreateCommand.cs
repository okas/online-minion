using ErrorOr;
using MediatR;
using OnlineMinion.Contracts.Common.Responses;

namespace OnlineMinion.Contracts.Common.Requests;

public interface ICreateCommand : IRequest<ErrorOr<ModelIdResp>>, IUpsertCommand { }
