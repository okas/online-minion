using MediatR;

namespace OnlineMinion.Contracts.Shared.Requests;

public interface IGetByIdRequest<out TResp> : IHasIntId, IRequest<TResp> { }
