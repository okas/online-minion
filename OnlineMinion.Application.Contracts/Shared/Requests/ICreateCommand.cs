using ErrorOr;
using OnlineMinion.Application.Contracts.Shared.Responses;

namespace OnlineMinion.Application.Contracts.Shared.Requests;

public interface ICreateCommand : IUpsertCommand<ErrorOr<ModelIdResp>>;
