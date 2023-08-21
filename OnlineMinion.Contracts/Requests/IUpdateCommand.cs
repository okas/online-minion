using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Requests;

public interface IUpdateCommand : IHasIntId, IRequest<ErrorOr<Updated>>, IUpsertCommand { }
