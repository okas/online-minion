using System.ComponentModel.DataAnnotations;
using MediatR;

namespace OnlineMinion.Contracts.Commands;

public record struct UpdateAccountSpecCmd(
    [property: Required] int    Id,
    [property: Required] string Name,
    [property: Required] string Group,
    string?                     Description
) : IRequest<int>;
