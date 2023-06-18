using MediatR;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public readonly record struct CheckAccountSpecUniqueNewReq(string Name) : IRequest<bool>;
