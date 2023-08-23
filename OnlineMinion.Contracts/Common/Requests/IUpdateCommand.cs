using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.Common.Requests;

public interface IUpdateCommand : IHasIntId, IRequest<ErrorOr<Updated>>, IUpsertCommand { }
