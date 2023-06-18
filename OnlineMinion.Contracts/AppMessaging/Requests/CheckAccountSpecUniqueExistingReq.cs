using MediatR;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public record CheckAccountSpecUniqueExistingReq(string Name, int ExceptId) : IRequest<bool>;
