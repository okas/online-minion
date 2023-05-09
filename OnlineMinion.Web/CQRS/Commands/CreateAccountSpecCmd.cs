using MediatR;
using OnlineMinion.Contracts.Commands;

namespace OnlineMinion.Web.CQRS.Commands;

internal sealed class CreateAccountSpecCmd : BaseCreateAccountSpecCmd, IRequest<bool>
{
    public CreateAccountSpecCmd() : base(string.Empty, string.Empty, null) { }

    public CreateAccountSpecCmd(string name, string group, string? description) : base(name, group, description) { }
}
