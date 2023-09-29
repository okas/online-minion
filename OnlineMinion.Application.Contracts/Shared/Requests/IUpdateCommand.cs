using ErrorOr;

namespace OnlineMinion.Application.Contracts.Shared.Requests;

public interface IUpdateCommand : IUpsertCommand<ErrorOr<Updated>>, IHasId;
