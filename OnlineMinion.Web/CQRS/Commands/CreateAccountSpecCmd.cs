using MediatR;

namespace OnlineMinion.Web.CQRS.Commands;

internal readonly record struct CreateAccountSpecCmd
    (string Name, string Group, string? Description) : IRequest<bool> { }
