using ErrorOr;
using OnlineMinion.Contracts.Shared.Responses;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface ICreateCommand : IUpsertCommand<ErrorOr<ModelIdResp>>;
