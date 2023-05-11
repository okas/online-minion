using MediatR;

namespace OnlineMinion.Contracts.Responses;

public interface IsSuccessResultCommand : IRequest<bool> { }
