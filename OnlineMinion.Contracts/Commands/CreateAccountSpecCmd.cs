using MediatR;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.Contracts.Commands;

public sealed class CreateAccountSpecCmd : BaseCreateAccountSpecCmd, IRequest<AccountSpecResp>
{
    public CreateAccountSpecCmd() : base(string.Empty, string.Empty, null) { }

    public CreateAccountSpecCmd(string name, string group, string? description) : base(name, group, description) { }

    public AccountSpecResp ToResponse(int id) => new(id, Name, Group, Description);
}
