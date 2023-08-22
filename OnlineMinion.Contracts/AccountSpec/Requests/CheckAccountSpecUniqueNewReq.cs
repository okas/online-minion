using MediatR;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public record CheckAccountSpecUniqueNewReq(string Name) : IRequest<bool>;