using MediatR;

namespace OnlineMinion.Contracts.AppMessaging.Requests;

public record CheckAccountSpecUniqueNewReq(string Name) : IRequest<bool>;
