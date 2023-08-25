using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IUpdateCommand : IHasIntId, IRequest<ErrorOr<Updated>>, IUpsertCommand { }
