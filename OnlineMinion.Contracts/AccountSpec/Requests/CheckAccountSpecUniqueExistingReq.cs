using ErrorOr;
using MediatR;

namespace OnlineMinion.Contracts.AccountSpec.Requests;

public record CheckAccountSpecUniqueExistingReq(string Name, int ExceptId) : IRequest<ErrorOr<Success>>;
