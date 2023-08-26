using ErrorOr;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IUpdateCommand : IUpsertCommand<ErrorOr<Updated>>, IHasIntId;
