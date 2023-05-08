using System.ComponentModel.DataAnnotations;
using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.Commands;

public readonly record struct CreateAccountSpecCmd
(
    [property: Required] string Name,
    [property: Required] string Group,
    string?                     Description
) : IRequest<AccountSpecResp>
{
    public AccountSpecResp ToResponse(int id) => new(id, Name, Group, Description);
}
