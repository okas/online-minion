using MediatR;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public readonly record struct CheckAccountSpecUniqueExistingReq(string Name, int Id) : IRequest<bool>;
