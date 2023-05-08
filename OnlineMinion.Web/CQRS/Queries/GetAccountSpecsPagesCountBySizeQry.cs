using MediatR;

namespace OnlineMinion.Web.CQRS.Queries;

internal readonly record struct GetAccountSpecsPagesCountBySizeQry(int Size) : IRequest<int?>;
